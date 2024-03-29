package util

import (
	"fmt"

	"github.com/spf13/viper"
)

type Config struct {
	AppId             string `mapstructure:"APP_ID"`
	GrpcServerAddress string `mapstructure:"GRPC_SERVER_ADDRESS"`
	JwtSecret         string `mapstructure:"JWT_SECRET"`
	BpfInterval       uint32 `mapstructure:"BPF_INTERVAL"`
}

func LoadConfig() (*Config, error) {
	var config Config
	viper.SetConfigFile("app.env")

	viper.AutomaticEnv()

	if err := viper.ReadInConfig(); err != nil {
		return nil, fmt.Errorf("failed to configuration file: %w", err)
	}

	if err := viper.Unmarshal(&config); err != nil {
		return nil, fmt.Errorf("failed to read configuration from file: %w", err)
	}

	return &config, nil
}
