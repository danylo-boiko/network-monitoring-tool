package cmd

import (
	"log"
	"net"
	"nmt_cli/pkg/bpf"
	"nmt_cli/pkg/grpc"
	"os"
	"os/signal"
	"syscall"

	"github.com/spf13/cobra"
)

var startCmd = &cobra.Command{
	Use:   "start",
	Short: "Start the processing",
	Args:  cobra.ExactArgs(1),
	Run: func(cmd *cobra.Command, args []string) {
		ifaceName := args[0]
		iface, err := net.InterfaceByName(ifaceName)
		if err != nil {
			log.Fatalf("Lookup network iface %q: %s", ifaceName, err)
		}

		loader := bpf.NewBpfLoader(iface)
		loader.Load()
		defer loader.Close()

		log.Printf("Attached XDP program to iface %q (index %d)", loader.Interface.Name, loader.Interface.Index)

		printStatsFlag, _ := cmd.Flags().GetBool("stats")

		client := grpc.NewGrpcClient()
		client.Connect("localhost:5144")
		defer client.CloseConnection()

		packetsHandler := bpf.NewPacketsHandler(client, loader.BpfObjects)
		go packetsHandler.Handle(printStatsFlag)

		quit := make(chan os.Signal, 1)
		signal.Notify(quit, syscall.SIGTERM, syscall.SIGINT)
		<-quit
	},
}

func init() {
	startCmd.Flags().Bool("stats", false, "Print stats of the network")
	rootCmd.AddCommand(startCmd)
}
