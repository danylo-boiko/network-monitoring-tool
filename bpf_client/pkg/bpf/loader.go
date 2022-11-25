package bpf

import (
	"fmt"
	"github.com/cilium/ebpf"
	"log"
	"net"
	"strings"
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
		BpfObjects: &bpfObjects{},
		Interface:  iface,
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

	for range ticker.C {
		statsMap := formatMap(loader.BpfObjects.StatsMap)
		log.Printf("Stats map:\n%s", statsMap)
		addressPacketsMap := formatMap(loader.BpfObjects.AddressPacketsMap)
		log.Printf("Address packets map:\n%s", addressPacketsMap)
		blockedIPsMap := formatMap(loader.BpfObjects.BlockedIpsMap)
		log.Printf("Blocked IPs map:\n%s", blockedIPsMap)
	}
}

func formatMap(m *ebpf.Map) string {
	var (
		sb  strings.Builder
		key []byte
		val uint32
	)
	iter := m.Iterate()
	for iter.Next(&key, &val) {
		keyStr := net.IP(key)
		sb.WriteString(fmt.Sprintf("\t%s => %d\n", keyStr, val))
	}
	return sb.String()
}
