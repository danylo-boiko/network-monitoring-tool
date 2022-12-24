package cmd

import (
	"nmt_cli/cmd/auth"
	"nmt_cli/internal"
	"nmt_cli/util"

	"github.com/spf13/cobra"
)

func NewCmdAuth(f *internal.Factory) *cobra.Command {
	var cmd = &cobra.Command{
		Use:   "auth <command>",
		Short: "Authenticate with network monitoring tool",
	}

	util.DisableAuthCheck(cmd)

	cmd.AddCommand(auth.NewCmdLogin(f))
	cmd.AddCommand(auth.NewCmdLogout(f))

	return cmd
}
