package grpc

import (
	"log"

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

func (gc *GrpcClient) Connect(target string) {
	var err error
	gc.connection, err = grpc.Dial(target, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("Failed to connect: %v", err)
	}

	gc.Packets = NewPacketsClient(gc.connection)
	gc.Auth = NewAuthClient(gc.connection)
}

func (gc *GrpcClient) CloseConnection() {
	gc.connection.Close()
}
