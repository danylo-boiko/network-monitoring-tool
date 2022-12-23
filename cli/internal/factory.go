package internal

import (
	"log"
	"nmt_cli/pkg/grpc"
	"nmt_cli/util"

	"github.com/denisbrodbeck/machineid"
)

type Factory struct {
	GrpcClient  *grpc.GrpcClient
	Credentials *util.Credentials

	Config    func() (*util.Config, error)
	MachineId func() (string, error)
}

func NewFactory() *Factory {
	f := &Factory{
		GrpcClient:  grpc.NewGrpcClient(),
		Credentials: newCredentials(),

		Config: configFunc(),
	}

	f.MachineId = machineIdFunc(f)

	return f
}

func configFunc() func() (*util.Config, error) {
	return func() (*util.Config, error) {
		return util.LoadConfig()
	}
}

func machineIdFunc(f *Factory) func() (string, error) {
	cfg, err := f.Config()
	if err != nil {
		log.Fatalln(err)
	}

	return func() (string, error) {
		return machineid.ProtectedID(cfg.AppId)
	}
}

func newCredentials() *util.Credentials {
	creds, err := util.ReadCredentials()
	if err != nil {
		log.Fatalln(err)
		return nil
	}

	return creds
}
