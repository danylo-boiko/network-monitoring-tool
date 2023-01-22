package cmd

import (
	"fmt"
	"nmt_cli/internal"
	"nmt_cli/util"
	"os"

	"github.com/MakeNowJust/heredoc"
	"github.com/spf13/cobra"
)

func NewCmdRoot() *cobra.Command {
	cmd := &cobra.Command{
		Use:           "nmt_cli <command> <subcommand> [flags]",
		Short:         "Network monitoring tool CLI",
		Long:          "nmt_cli - a simple CLI to inspect activity in your network",
		SilenceErrors: true,
		SilenceUsage:  true,
	}

	factory := internal.NewFactory()

	cmd.PersistentPreRunE = func(cmd *cobra.Command, args []string) error {
		if util.IsAuthCheckEnabled(cmd) {
			if err := factory.GrpcClient.ValidateAuth(); err != nil {
				fmt.Fprintln(os.Stdout, authHelp())
				return err
			}
		}
		return nil
	}

	cmd.AddCommand(NewCmdAuth(factory))
	cmd.AddCommand(NewCmdStart(factory))

	return cmd
}

func authHelp() string {
	return heredoc.Doc("To get started with CLI, please run: nmt_cli auth login")
}
