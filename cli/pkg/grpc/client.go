package grpc

import (
	"nmt_cli/util"

	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

type GrpcClient struct {
	connection *grpc.ClientConn
	Auth       AuthClient
	Packets    PacketsClient
	IpFilters  IpFiltersClient
}

func NewGrpcClient() *GrpcClient {
	return &GrpcClient{}
}

func (gc *GrpcClient) Connect(target string, creds *util.Credentials) (err error) {
	TSLCreds := insecure.NewCredentials()
	gc.connection, err = grpc.Dial(target, grpc.WithTransportCredentials(TSLCreds), grpc.WithPerRPCCredentials(creds))
	if err != nil {
		return err
	}

	gc.Auth = NewAuthClient(gc.connection)
	gc.Packets = NewPacketsClient(gc.connection)
	gc.IpFilters = NewIpFiltersClient(gc.connection)

	return nil
}

func (gc *GrpcClient) CloseConnection() {
	gc.connection.Close()
}
