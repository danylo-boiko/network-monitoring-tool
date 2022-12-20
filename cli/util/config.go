package util

import "github.com/spf13/viper"

type Config struct {
	GrpcServerAddress string `mapstructure:"GRPC_SERVER_ADDRESS"`
	BpfInterval       uint32 `mapstructure:"BPF_INTERVAL"`
}

func LoadConfig() (config Config, err error) {
	viper.SetConfigFile("app.env")

	viper.AutomaticEnv()

	err = viper.ReadInConfig()
	if err != nil {
		return
	}

	err = viper.Unmarshal(&config)
	return
}
