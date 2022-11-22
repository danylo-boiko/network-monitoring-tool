package bpf

import (
	"github.com/cilium/ebpf/link"
	"github.com/cilium/ebpf/rlimit"
	"log"
	"net"
)

//go:generate go run github.com/cilium/ebpf/cmd/bpf2go -cc $BPF_CLANG -cflags $BPF_CFLAGS bpf ./bpf_injector.c

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
		log.Fatal(err)
	}

	// Load pre-compiled programs into the kernel.
	if err := loadBpfObjects(loader.BpfObjects, nil); err != nil {
		log.Fatalf("loading objects: %s", err)
	}

	// Attach the program.
	xdp, err := link.AttachXDP(link.XDPOptions{
		Program:   loader.BpfObjects.BpfXdpHandler,
		Interface: loader.Interface.Index,
		Flags:     link.XDPGenericMode,
	})
	if err != nil {
		log.Fatalf("could not attach XDP program: %s", err)
	}
	loader.XdpLink = xdp

	log.Printf("Attached XDP program to iface %q (index %d)", loader.Interface.Name, loader.Interface.Index)
	log.Printf("Press Ctrl-C to exit and remove the program")
}

func (loader *Loader) Close() {
	loader.BpfObjects.Close()
	loader.XdpLink.Close()
}
