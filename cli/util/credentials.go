package util

import (
	"os"

	"gopkg.in/yaml.v3"
)

type Credentials struct {
	JwtToken string
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

func (creds *Credentials) Write() error {
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

func (creds *Credentials) Reset() {
	creds.JwtToken = ""
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
