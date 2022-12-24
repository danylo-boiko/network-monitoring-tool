package auth

import (
	"bufio"
	"context"
	"errors"
	"fmt"
	"nmt_cli/internal"
	"nmt_cli/pkg/grpc"
	"nmt_cli/util"
	"os"
	"strings"

	"github.com/spf13/cobra"
	"golang.org/x/crypto/ssh/terminal"
)

type LoginOptions struct {
	GrpcClient  *grpc.GrpcClient
	Credentials *util.Credentials

	Config    func() (*util.Config, error)
	MachineId func() (string, error)
}

func NewCmdLogin(f *internal.Factory) *cobra.Command {
	var opts = &LoginOptions{
		GrpcClient:  f.GrpcClient,
		Credentials: f.Credentials,
		Config:      f.Config,
		MachineId:   f.MachineId,
	}

	cmd := &cobra.Command{
		Use:   "login",
		Short: "Authenticate with a NMT host",
		Args:  cobra.ExactArgs(0),
		RunE: func(cmd *cobra.Command, args []string) error {
			return loginRun(opts)
		},
	}

	util.DisableAuthCheck(cmd)

	return cmd
}

func loginRun(opts *LoginOptions) error {
	cfg, err := opts.Config()
	if err != nil {
		return err
	}

	err = opts.GrpcClient.Connect(cfg.GrpcServerAddress, opts.Credentials)
	if err != nil {
		return err
	}
	defer opts.GrpcClient.CloseConnection()

	username, err := readLine("Input username: ")
	if err != nil {
		return err
	}

	password, err := readSecureLine("Input password: ")
	if err != nil {
		return err
	}

	hostname, err := os.Hostname()
	if err != nil {
		return err
	}

	machineStamp, err := opts.MachineId()
	if err != nil {
		return err
	}

	response, err := opts.GrpcClient.Auth.Login(context.Background(), &grpc.LoginRequest{
		Username:             username,
		Password:             password,
		Hostname:             hostname,
		MachineSpecificStamp: machineStamp,
	})
	if err != nil {
		return err
	}

	opts.Credentials.JwtToken = response.Token
	if err := opts.Credentials.Write(); err != nil {
		return err
	}

	return nil
}

func readLine(prompt string) (string, error) {
	fmt.Print(prompt)

	reader := bufio.NewReader(os.Stdin)
	input, err := reader.ReadString('\n')
	if err != nil {
		return "", err
	}

	return strings.TrimSuffix(input, "\n"), nil
}

func readSecureLine(prompt string) (string, error) {
	fd := int(os.Stdin.Fd())
	if !terminal.IsTerminal(fd) {
		return "", errors.New("non terminal input")
	}

	fmt.Print(prompt)

	input, err := terminal.ReadPassword(fd)
	if err != nil {
		return "", err
	}

	fmt.Print("\n")

	return string(input), nil
}
