package cmd

import (
	"fmt"
	"os"

	"github.com/spf13/cobra"
)

var rootCmd = &cobra.Command{
	Use:     "bpf_client",
	Version: "0.0.1",
	Short:   "bpf_client - a simple CLI to inspect activity in your network",
	Args:    cobra.ExactArgs(1),
	Run: func(cmd *cobra.Command, args []string) {
	},
}

func Execute() {
	if err := rootCmd.Execute(); err != nil {
		fmt.Fprintf(os.Stderr, "Ooops. Error: %s\n", err)
		os.Exit(1)
	}
}
