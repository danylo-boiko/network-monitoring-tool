package main

import (
	"nmt_cli/cmd"
	"os"
)

func main() {
	rootCmd := cmd.NewCmdRoot()

	if err := rootCmd.Execute(); err != nil {
		os.Exit(1)
	}
}
