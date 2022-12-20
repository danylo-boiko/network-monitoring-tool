package cmd

import (
	"nmt_cli/internal"

	"github.com/spf13/cobra"
)

func NewCmdRoot() *cobra.Command {
	cmd := &cobra.Command{
		Use:   "nmt_cli <command> <subcommand> [flags]",
		Short: "Network monitoring tool CLI",
		Long:  "nmt_cli - a simple CLI to inspect activity in your network",
	}

	factory := internal.NewFactory()

	cmd.AddCommand(NewCmdAuth(factory))
	cmd.AddCommand(newCmdStart(factory))

	return cmd
}
