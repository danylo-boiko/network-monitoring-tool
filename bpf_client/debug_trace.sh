#!/bin/sh

sudo mount -t debugfs none /sys/kernel/debug
sudo xdp-loader status
echo "Tracing:";
sudo cat /sys/kernel/debug/tracing/trace_pipe
