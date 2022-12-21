package grpc

import (
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

type GrpcClient struct {
	connection *grpc.ClientConn
	Auth       AuthClient
	Packets    PacketsClient
}

func NewGrpcClient() *GrpcClient {
	return &GrpcClient{}
}

func (gc *GrpcClient) Connect(target string) error {
	var err error
	gc.connection, err = grpc.Dial(target, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		return err
	}

	gc.Packets = NewPacketsClient(gc.connection)
	gc.Auth = NewAuthClient(gc.connection)

	return nil
}

func (gc *GrpcClient) CloseConnection() {
	gc.connection.Close()
}
