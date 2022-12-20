package main

import (
	"fmt"
	"nmt_cli/cmd"
	"os"
)

func main() {
	rootCmd := cmd.NewCmdRoot()

	if err := rootCmd.Execute(); err != nil {
		fmt.Fprintf(os.Stderr, "Ooops. Error: %s\n", err)
		os.Exit(1)
	}
}
