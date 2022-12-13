package bpf

import (
	"log"
	"net"
	"time"

	"github.com/cilium/ebpf/link"
	"github.com/cilium/ebpf/rlimit"
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

func (loader *Loader) PrintStats() {
	ticker := time.NewTicker(1 * time.Second)
	defer ticker.Stop()

	printFormatter := NewPrintFormatter()

	for range ticker.C {
		statsMap := printFormatter.formatStatsMap(loader.BpfObjects.StatsMap)
		log.Printf("Stats:\n%s", statsMap)

		addressPacketsMap := printFormatter.formatIPsMap(loader.BpfObjects.AddressPacketsMap)
		if addressPacketsMap != "" {
			log.Printf("Address packets:\n%s", addressPacketsMap)
		}

		blockedIPsMap := printFormatter.formatIPsMap(loader.BpfObjects.BlockedIpsMap)
		if blockedIPsMap != "" {
			log.Printf("Blocked IPs:\n%s", blockedIPsMap)
		}
	}
}
