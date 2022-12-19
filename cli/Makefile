.PHONY: build generate clean
.DEFAULT_GOAL = build

# main
BIN_DIR=bin
BIN_NAME=nmt_cli

# compiler and flags
CLANG := clang
CFLAGS := -g -O2 -Wall -Wextra $(CFLAGS)

generate: export BPF_CLANG := $(CLANG)
generate: export BPF_CFLAGS := $(CFLAGS)
generate:
	protoc packets.proto --proto_path=./../server/Nmt.Grpc/Protos/ --go_out=. --go-grpc_out=.
	cd pkg/bpf && go generate -v ./...

build: generate
	mkdir -p $(BIN_DIR)
	go build -o ${BIN_DIR}/${BIN_NAME} ./main.go

clean:
	find . -name "*.o" -type f -delete
	rm -r ${BIN_DIR}
