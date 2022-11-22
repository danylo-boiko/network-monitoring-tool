#!/bin/sh

sudo mount -t debugfs none /sys/kernel/debug
make clean
make build
sudo ./bin/bpf_client eth0

while getopts "st" opt; do
  case $opt in
    s)
      sudo xdp-loader status
      ;;
    t)
      echo "Tracing:";
      sudo cat /sys/kernel/debug/tracing/trace_pipe
      ;;
  esac
done