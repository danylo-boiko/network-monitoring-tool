package cmd

import (
	"log"
	"net"
	"os"
	"os/signal"
	"syscall"

	"github.com/spf13/cobra"
	"nmt_cli/pkg/bpf"
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

		statsFlag, _ := cmd.Flags().GetBool("stats")
		if statsFlag {
			loader.CollectStats()
		}

		quit := make(chan os.Signal, 1)
		signal.Notify(quit, syscall.SIGTERM, syscall.SIGINT)
		<-quit
	},
}

func init() {
	startCmd.Flags().Bool("stats", false, "Print stats of the network")
	rootCmd.AddCommand(startCmd)
}