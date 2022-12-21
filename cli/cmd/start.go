package cmd

import (
	"context"
	"errors"
	"log"
	"net"
	"nmt_cli/internal"
	"nmt_cli/pkg/bpf"
	"nmt_cli/pkg/grpc"
	"nmt_cli/util"
	"time"

	"github.com/cilium/ebpf"
	"github.com/spf13/cobra"
)

type StartOptions struct {
	GrpcClient *grpc.GrpcClient
	Iface      *net.Interface
	PrintStats bool

	Config func() (util.Config, error)
}

func newCmdStart(f *internal.Factory) *cobra.Command {
	var opts = &StartOptions{
		GrpcClient: f.GrpcClient,
		Config:     f.Config,
	}

	var cmd = &cobra.Command{
		Use:   "start <protocol>",
		Short: "Start the processing of xpd packets",
		Args:  cobra.ExactArgs(1),
		RunE: func(cmd *cobra.Command, args []string) error {
			var err error

			opts.Iface, err = net.InterfaceByName(args[0])
			if err != nil {
				return err
			}

			opts.PrintStats, err = cmd.Flags().GetBool("stats")
			if err != nil {
				return err
			}

			return startRun(opts)
		},
	}

	cmd.Flags().Bool("stats", false, "Print stats of the network to console")

	return cmd
}

func startRun(opts *StartOptions) error {
	cfg, err := opts.Config()
	if err != nil {
		return err
	}

	loader := bpf.NewBpfLoader(opts.Iface)
	loader.Load()
	defer loader.Close()

	log.Printf("Attached XDP program to iface %q (index %d)", loader.Interface.Name, loader.Interface.Index)

	err = opts.GrpcClient.Connect(cfg.GrpcServerAddress)
	if err != nil {
		return err
	}

	defer opts.GrpcClient.CloseConnection()

	grpcTicker := time.NewTicker(time.Duration(cfg.BpfInterval) * time.Second)
	defer grpcTicker.Stop()

	for range grpcTicker.C {
		packets := make([]*grpc.PacketModel, 0, cfg.BpfInterval*loader.BpfObjects.PacketsQueue.MaxEntries())

		for bpfLookup := time.Now(); time.Since(bpfLookup) < time.Second; {
			packet := bpf.NewBpfPacket()

			if err := loader.BpfObjects.PacketsQueue.LookupAndDelete(nil, packet); err != nil {
				if !errors.Is(err, ebpf.ErrKeyNotExist) {
					log.Fatalf("Lookup should have failed with error, %v, instead error is %v", ebpf.ErrKeyNotExist, err)
				} else {
					continue
				}
			}

			if opts.PrintStats {
				log.Printf("IP: %d, size: %d, status: %d, protocol: %d", packet.Ip, packet.Size, packet.Status, packet.Protocol)
			}

			packets = append(packets, &grpc.PacketModel{
				Ip:       packet.Ip,
				Size:     packet.Size,
				Protocol: uint32(packet.Protocol),
				Status:   uint32(packet.Status),
			})
		}

		_, err := opts.GrpcClient.Packets.AddPackets(context.Background(), &grpc.AddPacketsRequest{Packets: packets})
		if err != nil {
			return err
		}
	}

	return nil
}
