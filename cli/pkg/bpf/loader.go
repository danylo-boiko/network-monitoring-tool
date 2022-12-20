package bpf

import (
	"log"
	"net"

	"github.com/cilium/ebpf/link"
	"github.com/cilium/ebpf/rlimit"
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

func (ldr *Loader) Load() {
	if err := rlimit.RemoveMemlock(); err != nil {
		log.Fatalf("Could not remove momory lock: %s", err)
	}

	if err := loadBpfObjects(ldr.BpfObjects, nil); err != nil {
		log.Fatalf("Could not load bpf objects: %s", err)
	}

	var err error
	if ldr.XdpLink, err = link.AttachXDP(link.XDPOptions{
		Program:   ldr.BpfObjects.BpfXdpHandler,
		Interface: ldr.Interface.Index,
		Flags:     link.XDPGenericMode,
	}); err != nil {
		log.Fatalf("Could not attach XDP program: %s", err)
	}
}

func (ldr *Loader) Close() {
	ldr.BpfObjects.Close()
	ldr.XdpLink.Close()
}

func NewBpfPacket() *bpfPacket {
	return &bpfPacket{}
}
