package util

import (
	"fmt"
	"os"

	"github.com/dgrijalva/jwt-go"
	"golang.org/x/net/context"
	"gopkg.in/yaml.v3"
)

type Credentials struct {
	AccessToken  string
	RefreshToken string
}

func (creds *Credentials) GetRequestMetadata(ctx context.Context, uri ...string) (map[string]string, error) {
	return map[string]string{
		"authorization": "Bearer " + creds.AccessToken,
	}, nil
}

func (creds *Credentials) RequireTransportSecurity() bool {
	return false
}

func (creds *Credentials) Reset() error {
	return creds.Update("", "")
}

func (creds *Credentials) Update(accessToken string, refreshToken string) error {
	creds.AccessToken = accessToken
	creds.RefreshToken = refreshToken

	if err := creds.write(); err != nil {
		return err
	}

	return nil
}

func ReadCredentials() (*Credentials, error) {
	if err := createIfNotExists(); err != nil {
		return nil, err
	}

	file, err := os.ReadFile(getCredsFilePath())
	if err != nil {
		return nil, err
	}

	var creds Credentials
	if err := yaml.Unmarshal(file, &creds); err != nil {
		return nil, err
	}

	return &creds, nil
}

func GetClaims(token string, cfg *Config) (*jwt.StandardClaims, error) {
	parsedToken, err := jwt.ParseWithClaims(token, &jwt.StandardClaims{}, func(token *jwt.Token) (interface{}, error) {
		_, success := token.Method.(*jwt.SigningMethodHMAC)
		if !success {
			return nil, fmt.Errorf("unexpected token signing method")
		}

		return []byte(cfg.JwtSecret), nil
	})

	if err != nil {
		return nil, fmt.Errorf("invalid token: %w", err)
	}

	claims, success := parsedToken.Claims.(*jwt.StandardClaims)
	if !success {
		return nil, fmt.Errorf("invalid token claims")
	}

	return claims, nil
}

func (creds *Credentials) write() error {
	if err := createIfNotExists(); err != nil {
		return err
	}

	data, err := yaml.Marshal(&creds)
	if err != nil {
		return err
	}

	if err := os.WriteFile(getCredsFilePath(), data, os.ModePerm); err != nil {
		return err
	}

	return nil
}

func createIfNotExists() error {
	if err := os.MkdirAll(getCredsFolder(), os.ModePerm); err != nil {
		return err
	}

	filePath := getCredsFilePath()
	_, err := os.Stat(filePath)
	if os.IsNotExist(err) {
		file, err := os.Create(filePath)
		if err != nil {
			return err
		}
		defer file.Close()
	}

	return nil
}

func getCredsFolder() string {
	homeDir, _ := os.UserHomeDir()
	return homeDir + "/.nmt"
}

func getCredsFilePath() string {
	return getCredsFolder() + "/credentials.yaml"
}
