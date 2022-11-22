package cli

import (
	"bpf_client/pkg/bpf"
	"fmt"
	"github.com/cilium/ebpf"
	"log"
	"net"
	"strings"
	"time"
)

type Tracer struct {
	BpfLoader *bpf.Loader
}

func NewBpfTracer(loader *bpf.Loader) *Tracer {
	return &Tracer{BpfLoader: loader}
}

func (tracer *Tracer) TraceContent() {
	defer tracer.BpfLoader.Close()

	ticker := time.NewTicker(1 * time.Second)
	defer ticker.Stop()
	for range ticker.C {
		statsMap := formatStatsMap(tracer.BpfLoader.BpfObjects.StatsMap)
		log.Printf("Stats map:\n%s", statsMap)
		addressPacketsMap := formatIPsMap(tracer.BpfLoader.BpfObjects.AddressPacketsMap)
		log.Printf("Address packets map:\n%s", addressPacketsMap)
		blockedIPsMap := formatIPsMap(tracer.BpfLoader.BpfObjects.BlockedIpsMap)
		log.Printf("Blocked IPs map:\n%s", blockedIPsMap)
	}
}

func formatStatsMap(m *ebpf.Map) string {
	var (
		sb  strings.Builder
		key byte
		val uint32
	)
	iter := m.Iterate()
	for iter.Next(&key, &val) {
		sb.WriteString(fmt.Sprintf("\t%d => %d\n", key, val))
	}
	return sb.String()
}

func formatIPsMap(m *ebpf.Map) string {
	var (
		sb  strings.Builder
		key []byte
		val uint32
	)
	iter := m.Iterate()
	for iter.Next(&key, &val) {
		sourceIP := net.IP(key)
		packetCount := val
		sb.WriteString(fmt.Sprintf("\t%s => %d\n", sourceIP, packetCount))
	}
	return sb.String()
}
