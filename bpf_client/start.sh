#!/bin/sh

./scripts/unload_injector.sh
sudo mount -t debugfs none /sys/kernel/debug
make
./scripts/load_injector.sh

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