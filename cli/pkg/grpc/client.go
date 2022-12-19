package grpc

import (
	"log"

	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
)

type GrpcClient struct {
	connection *grpc.ClientConn
	Packets    PacketsClient
}

func NewGrpcClient() *GrpcClient {
	return &GrpcClient{}
}

func (grpcClient *GrpcClient) Connect(target string) {
	var err error
	grpcClient.connection, err = grpc.Dial(target, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("Failed to connect: %v", err)
	}

	grpcClient.Packets = NewPacketsClient(grpcClient.connection)
}

func (grpcClient *GrpcClient) CloseConnection() {
	grpcClient.connection.Close()
}
