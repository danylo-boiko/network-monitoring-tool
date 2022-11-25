package bpf

import (
	"fmt"
	"net"
	"strings"

	"github.com/cilium/ebpf"
)

type PrintFormatter struct {
	StatsLabels map[byte]string
}

func NewPrintFormatter() *PrintFormatter {
	return &PrintFormatter{
		StatsLabels: getStatsLabels(),
	}
}

func getStatsLabels() map[byte]string {
	m := make(map[byte]string)

	m[0] = "Passed packets"
	m[1] = "Dropped packets"
	m[2] = "Passed pkt size"
	m[3] = "TCP protocol"
	m[4] = "UDP protocol"
	m[5] = "ICMP protocol"
	m[6] = "Other protocol"

	return m
}

func (pf *PrintFormatter) formatStatsMap(m *ebpf.Map) string {
	var (
		sb  strings.Builder
		key byte
		val uint32
	)
	iter := m.Iterate()
	for iter.Next(&key, &val) {
		sb.WriteString(fmt.Sprintf("\t%s\t => %d\n", pf.StatsLabels[key], val))
	}
	return sb.String()
}

func (pf *PrintFormatter) formatIPsMap(m *ebpf.Map) string {
	var (
		sb  strings.Builder
		key []byte
		val uint32
	)
	iter := m.Iterate()
	for iter.Next(&key, &val) {
		ip := net.IP(key)
		sb.WriteString(fmt.Sprintf("\t%s => %d\n", ip, val))
	}
	return sb.String()
}
