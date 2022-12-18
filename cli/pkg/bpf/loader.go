package bpf

import (
	"errors"
	"github.com/cilium/ebpf"
	"github.com/cilium/ebpf/link"
	"github.com/cilium/ebpf/rlimit"
	"log"
	"net"
)

//go:generate go run github.com/cilium/ebpf/cmd/bpf2go -cc $BPF_CLANG -cflags $BPF_CFLAGS -type Packet bpf ./kernel/bpf_injector.c

type Loader struct {
	Interface  *net.Interface
	BpfObjects *bpfObjects
	XdpLink    link.Link
}

func NewBpfLoader(iface *net.Interface) *Loader {
	return &Loader{
		Interface:  iface,
		BpfObjects: &bpfObjects{},
		XdpLink:    nil,
	}
}

func (loader *Loader) Load() {
	if err := rlimit.RemoveMemlock(); err != nil {
		log.Fatalf("Could not remove momory lock: %s", err)
	}

	if err := loadBpfObjects(loader.BpfObjects, nil); err != nil {
		log.Fatalf("Could not load bpf objects: %s", err)
	}

	var err error
	if loader.XdpLink, err = link.AttachXDP(link.XDPOptions{
		Program:   loader.BpfObjects.BpfXdpHandler,
		Interface: loader.Interface.Index,
		Flags:     link.XDPGenericMode,
	}); err != nil {
		log.Fatalf("Could not attach XDP program: %s", err)
	}
}

func (loader *Loader) Close() {
	loader.BpfObjects.Close()
	loader.XdpLink.Close()
}

func (loader *Loader) CollectStats() {
	var packet bpfPacket
	for {
		if err := loader.BpfObjects.PacketsQueue.LookupAndDelete(nil, &packet); err != nil {
			if !errors.Is(err, ebpf.ErrKeyNotExist) {
				log.Fatalf("Lookup should have failed with error, %v, instead error is %v",
					ebpf.ErrKeyNotExist,
					err)
			} else {
				continue
			}
		}

		log.Printf("IP: %d, Size: %d, status: %d, protocol: %d",
			packet.Ip,
			packet.Size,
			packet.Status,
			packet.Protocol)
	}
}
