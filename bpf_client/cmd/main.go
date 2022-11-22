package main

import (
	"log"
	"net"
	"os"

	"bpf_client/pkg/bpf"
)

func main() {
	if len(os.Args) < 2 {
		log.Fatalf("Please specify a network interface")
	}

	// Look up the network interface by name.
	ifaceName := os.Args[1]
	iface, err := net.InterfaceByName(ifaceName)
	if err != nil {
		log.Fatalf("lookup network iface %q: %s", ifaceName, err)
	}

	loader := bpf.NewBpfLoader(iface)
	loader.Load()
}
