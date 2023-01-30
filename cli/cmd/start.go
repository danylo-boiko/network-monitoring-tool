package cmd

import (
	"context"
	"encoding/binary"
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
	"google.golang.org/protobuf/types/known/timestamppb"
)

type StartOptions struct {
	GrpcClient *grpc.GrpcClient
	Iface      *net.Interface
	PrintStats bool

	Credentials func() (*util.Credentials, error)
	Config      func() (*util.Config, error)
}

func NewCmdStart(f *internal.Factory) *cobra.Command {
	var opts = &StartOptions{
		GrpcClient:  f.GrpcClient,
		Credentials: f.Credentials,
		Config:      f.Config,
	}

	var cmd = &cobra.Command{
		Use:   "start <protocol>",
		Short: "Start the processing of xpd packets",
		Args:  cobra.ExactArgs(1),
		RunE: func(cmd *cobra.Command, args []string) (err error) {
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

	creds, err := opts.Credentials()
	if err != nil {
		return nil
	}

	loader := bpf.NewBpfLoader(opts.Iface)
	if err := loader.Load(); err != nil {
		return err
	}
	defer loader.Close()

	log.Printf("Attached XDP program to iface %q (index %d)", loader.Interface.Name, loader.Interface.Index)

	err = opts.GrpcClient.Connect(cfg.GrpcServerAddress, creds)
	if err != nil {
		return err
	}
	defer opts.GrpcClient.CloseConnection()

	response, err := opts.GrpcClient.IpFilters.GetIpFilters(context.Background(), &grpc.GetIpFiltersRequest{})
	if err != nil {
		return err
	}

	for _, ipFilter := range response.IpFilters {
		if err := loader.BpfObjects.IpFiltersMap.Put(ipFilter.Ip, ipFilter.FilterAction); err != nil {
			return err
		}
	}

	grpcTicker := time.NewTicker(time.Duration(cfg.BpfInterval) * time.Second)
	defer grpcTicker.Stop()

	for range grpcTicker.C {
		packets := make([]*grpc.PacketModel, 0, cfg.BpfInterval*loader.BpfObjects.PacketsQueue.MaxEntries())

		for bpfLookup := time.Now(); time.Since(bpfLookup) < time.Second; {
			packet := bpf.NewBpfPacket()

			if err := loader.BpfObjects.PacketsQueue.LookupAndDelete(nil, packet); err != nil {
				if !errors.Is(err, ebpf.ErrKeyNotExist) {
					log.Printf("Lookup should have failed with error, %v, instead error is %v", ebpf.ErrKeyNotExist, err)
				} else {
					continue
				}
			}

			if opts.PrintStats {
				log.Printf("IP: %s, size: %d, status: %d, protocol: %d", intToIPv4(packet.Ip), packet.Size, packet.Status, packet.Protocol)
			}

			packets = append(packets, &grpc.PacketModel{
				Ip:        packet.Ip,
				Size:      packet.Size,
				Protocol:  uint32(packet.Protocol),
				Status:    uint32(packet.Status),
				CreatedAt: timestamppb.Now(),
			})
		}

		if len(packets) > 0 {
			addPacketsRequest := &grpc.AddPacketsRequest{
				Packets: packets,
			}

			if _, err := opts.GrpcClient.Packets.AddPackets(context.Background(), addPacketsRequest); err != nil {
				return err
			}
		}
	}

	return nil
}

func intToIPv4(ipaddr uint32) net.IP {
	ip := make(net.IP, net.IPv4len)
	binary.BigEndian.PutUint32(ip, ipaddr)
	return ip
}
