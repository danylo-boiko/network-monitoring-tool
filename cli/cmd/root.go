package cmd

import (
	"errors"
	"nmt_cli/internal"
	"nmt_cli/util"

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

	cmd.PersistentPreRunE = func(cmd *cobra.Command, args []string) error {
		if util.IsAuthCheckEnabled(cmd) && !util.IsAuthValid() {
			return errors.New(authHelp())
		}

		return nil
	}

	factory := internal.NewFactory()

	cmd.AddCommand(NewCmdAuth(factory))
	cmd.AddCommand(newCmdStart(factory))

	return cmd
}

func authHelp() string {
	return heredoc.Doc("To get started with CLI, please run: nmt_cli auth login")
}
