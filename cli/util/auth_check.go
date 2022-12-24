package util

import (
	"errors"

	"github.com/spf13/cobra"
)

func DisableAuthCheck(cmd *cobra.Command) {
	if cmd.Annotations == nil {
		cmd.Annotations = map[string]string{}
	}

	cmd.Annotations["skipAuthCheck"] = "true"
}

func ValidateAuth() error {
	creds, err := ReadCredentials()
	if err != nil {
		return err
	}

	if creds.JwtToken == "" {
		return errors.New("you are not authenticated (token is empty)")
	}

	cfg, err := LoadConfig()
	if err != nil {
		return err
	}

	claims, err := creds.GetClaims(cfg)
	if err != nil {
		return err
	}

	if err = claims.Valid(); err != nil {
		return err
	}

	return nil
}

func IsAuthCheckEnabled(cmd *cobra.Command) bool {
	switch cmd.Name() {
	case "help", cobra.ShellCompRequestCmd, cobra.ShellCompNoDescRequestCmd:
		return false
	}

	for c := cmd; c.Parent() != nil; c = c.Parent() {
		if c.Annotations != nil && c.Annotations["skipAuthCheck"] == "true" {
			return false
		}
	}

	return true
}
