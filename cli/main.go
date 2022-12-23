package main

import (
	"fmt"
	"io"
	"nmt_cli/cmd"
	"os"
	"strings"

	"github.com/spf13/cobra"
)

func main() {
	rootCmd := cmd.NewCmdRoot()

	if rcmd, err := rootCmd.ExecuteC(); err != nil {
		printError(os.Stdout, rcmd, err)
		os.Exit(1)
	}
}

func printError(out io.Writer, cmd *cobra.Command, err error) {
	fmt.Fprintln(out, err)

	if strings.HasPrefix(err.Error(), "unknown command") || strings.HasPrefix(err.Error(), "unknown flag") {
		if !strings.HasSuffix(err.Error(), "\n") {
			fmt.Fprintln(out)
		}
		fmt.Fprintln(out, cmd.UsageString())
	}
}
