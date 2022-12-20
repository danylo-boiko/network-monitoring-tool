package auth

import (
	"nmt_cli/internal"

	"github.com/spf13/cobra"
)

func NewCmdLogin(f *internal.Factory) *cobra.Command {
	cmd := &cobra.Command{
		Use:   "login",
		Short: "Authenticate with a NMT host",
		Args:  cobra.ExactArgs(0),
		Run: func(cmd *cobra.Command, args []string) {
		},
	}

	return cmd
}
