package bpf

import (
	"fmt"
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

func (ldr *Loader) Load() (err error) {
	if err := rlimit.RemoveMemlock(); err != nil {
		return fmt.Errorf("could not remove momory lock: %w", err)
	}

	if err := loadBpfObjects(ldr.BpfObjects, nil); err != nil {
		return fmt.Errorf("could not load bpf objects: %w", err)
	}

	if ldr.XdpLink, err = link.AttachXDP(link.XDPOptions{
		Program:   ldr.BpfObjects.BpfXdpHandler,
		Interface: ldr.Interface.Index,
		Flags:     link.XDPGenericMode,
	}); err != nil {
		return fmt.Errorf("could not attach XDP program: %w", err)
	}

	return nil
}

func (ldr *Loader) Close() {
	ldr.BpfObjects.Close()
	ldr.XdpLink.Close()
}

func NewBpfPacket() *bpfPacket {
	return &bpfPacket{}
}
