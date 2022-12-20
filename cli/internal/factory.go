package internal

import (
	"nmt_cli/pkg/grpc"
	"nmt_cli/util"
)

type Factory struct {
	GrpcClient *grpc.GrpcClient

	Config func() (util.Config, error)
}

func NewFactory() *Factory {
	factory := &Factory{
		Config: configFunc(),
	}

	factory.GrpcClient = grpc.NewGrpcClient()

	return factory
}

func configFunc() func() (util.Config, error) {
	var config util.Config
	var configError error

	return func() (util.Config, error) {
		config, configError = util.LoadConfig()
		return config, configError
	}
}
