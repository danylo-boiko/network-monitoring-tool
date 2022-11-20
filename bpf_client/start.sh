#!/bin/sh

./scripts/unload_injector.sh
make
./scripts/load_injector.sh

while getopts "s" opt; do
  case $opt in
    s)
      sudo xdp-loader status
      ;;
  esac
done