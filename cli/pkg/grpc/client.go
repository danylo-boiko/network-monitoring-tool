package grpc

import (
	"context"
	"errors"
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

func (gc *GrpcClient) ValidateAuth() error {
	creds, err := util.ReadCredentials()
	if err != nil {
		return err
	}

	if creds.AccessToken == "" {
		return errors.New("you are not authenticated (token is empty)")
	}

	cfg, err := util.LoadConfig()
	if err != nil {
		return err
	}

	accessTokenClaims, _ := util.GetClaims(creds.AccessToken, cfg)
	if accessTokenClaims == nil || accessTokenClaims.Valid() != nil {
		refreshTokenClaims, err := util.GetClaims(creds.RefreshToken, cfg)
		if err != nil {
			return err
		}

		if err = refreshTokenClaims.Valid(); err != nil {
			return err
		}

		err = gc.Connect(cfg.GrpcServerAddress, creds)
		if err != nil {
			return err
		}
		defer gc.CloseConnection()

		response, err := gc.Auth.RefreshToken(context.Background(), &RefreshTokenRequest{
			AccessToken:  creds.AccessToken,
			RefreshToken: creds.RefreshToken,
		})
		if err != nil {
			return err
		}

		if err := creds.Update(response.AccessToken, response.RefreshToken); err != nil {
			return err
		}
	}

	return nil
}

func (gc *GrpcClient) CloseConnection() {
	gc.connection.Close()
}
