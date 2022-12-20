package auth

import (
	"errors"
	"nmt_cli/internal"
	"nmt_cli/util"

	"github.com/spf13/cobra"
)

type LogoutOptions struct {
	Credentials *util.Credentials
}

func NewCmdLogout(f *internal.Factory) *cobra.Command {
	var opts = &LogoutOptions{
		Credentials: f.Credentials,
	}

	cmd := &cobra.Command{
		Use:   "logout",
		Short: "Log out of a NMT host",
		Args:  cobra.ExactArgs(0),
		RunE: func(cmd *cobra.Command, args []string) error {
			return logoutRun(opts)
		},
	}

	return cmd
}

func logoutRun(opts *LogoutOptions) error {
	if opts.Credentials.JwtToken == "" {
		return errors.New("you are not authenticated")
	}

	opts.Credentials.Reset()
	if err := opts.Credentials.Write(); err != nil {
		return err
	}

	return nil
}
