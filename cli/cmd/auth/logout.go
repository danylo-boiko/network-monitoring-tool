package auth

import (
	"nmt_cli/internal"

	"github.com/spf13/cobra"
)

func NewCmdLogout(f *internal.Factory) *cobra.Command {
	cmd := &cobra.Command{
		Use:   "logout",
		Short: "Log out of a NMT host",
		Args:  cobra.ExactArgs(0),
		Run: func(cmd *cobra.Command, args []string) {
		},
	}

	return cmd
}
