package bpf

import (
	"context"
	"errors"
	"log"
	"nmt_cli/pkg/grpc"
	"time"

	"github.com/cilium/ebpf"
)

type PacketsHandler struct {
	grpcClient *grpc.GrpcClient
	bpfObjects *bpfObjects
}

func NewPacketsHandler(grpcClient *grpc.GrpcClient, bpfObjects *bpfObjects) *PacketsHandler {
	return &PacketsHandler{
		grpcClient: grpcClient,
		bpfObjects: bpfObjects,
	}
}

func (ph *PacketsHandler) Handle(printStats bool) {
	grpcTicker := time.NewTicker(3 * time.Second)
	defer grpcTicker.Stop()

	for range grpcTicker.C {
		packets := make([]*grpc.PacketModel, 0, 3*ph.bpfObjects.PacketsQueue.MaxEntries())

		for bpfLookup := time.Now(); time.Since(bpfLookup) < time.Second; {
			var packet bpfPacket
			if err := ph.bpfObjects.PacketsQueue.LookupAndDelete(nil, &packet); err != nil {
				if !errors.Is(err, ebpf.ErrKeyNotExist) {
					log.Fatalf("Lookup should have failed with error, %v, instead error is %v", ebpf.ErrKeyNotExist, err)
				} else {
					continue
				}
			}

			if printStats {
				log.Printf("IP: %d, size: %d, status: %d, protocol: %d", packet.Ip, packet.Size, packet.Status, packet.Protocol)
			}

			packets = append(packets, &grpc.PacketModel{
				Ip:       packet.Ip,
				Size:     packet.Size,
				Protocol: uint32(packet.Protocol),
				Status:   uint32(packet.Status),
			})
		}

		_, err := ph.grpcClient.Packets.AddPackets(context.Background(), &grpc.AddPacketsRequest{Packets: packets})
		if err != nil {
			log.Fatalf("Failed to add packets: %v", err)
		}
	}
}
