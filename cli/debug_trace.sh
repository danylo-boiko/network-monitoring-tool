#!/bin/sh

sudo mount -t debugfs none /sys/kernel/debug
sudo xdp-loader status
echo "Debug tracing:";
sudo cat /sys/kernel/debug/tracing/trace_pipe
