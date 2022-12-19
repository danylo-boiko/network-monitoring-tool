package bpf

import (
	"errors"
	"github.com/cilium/ebpf"
	"github.com/cilium/ebpf/link"
	"github.com/cilium/ebpf/rlimit"
	"log"
	"net"
	"nmt_cli/pkg/grpc"
	"time"
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

func (loader *Loader) CollectStats(printStats bool) {
	grpcTicker := time.NewTicker(3 * time.Second)
	defer grpcTicker.Stop()

	for range grpcTicker.C {
		packets := make([]grpc.PacketModel, 0, 3*loader.BpfObjects.PacketsQueue.MaxEntries())

		for bpfLookup := time.Now(); time.Since(bpfLookup) < time.Second; {
			var packet bpfPacket
			if err := loader.BpfObjects.PacketsQueue.LookupAndDelete(nil, &packet); err != nil {
				if !errors.Is(err, ebpf.ErrKeyNotExist) {
					log.Fatalf("Lookup should have failed with error, %v, instead error is %v", ebpf.ErrKeyNotExist, err)
				} else {
					continue
				}
			}

			if printStats {
				log.Printf("IP: %d, size: %d, status: %d, protocol: %d", packet.Ip, packet.Size, packet.Status, packet.Protocol)
			}

			packets = append(packets, grpc.PacketModel{
				Ip:       packet.Ip,
				Size:     packet.Size,
				Protocol: uint32(packet.Protocol),
				Status:   uint32(packet.Status),
			})
		}

		log.Print(packets)
	}
}
