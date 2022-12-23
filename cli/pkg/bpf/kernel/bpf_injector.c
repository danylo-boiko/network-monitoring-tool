// +build ignore

#include <stdbool.h>
#include <linux/bpf.h>
#include <bpf/bpf_helpers.h>
#include <netinet/in.h>
#include <linux/if_ether.h>
#include <linux/if_packet.h>
#include <linux/ip.h>

#include "bpf_includes.h"

enum PacketStatus {
    Passed = 1,
    Dropped = 2
};

struct Packet {
    uint ip;
    uint size;
    uint8_t protocol;
    uint8_t status;
};

void trace_packet(struct Packet* packet) {
    bpf_printk("Size: %u, status: %u, protocol: %u", packet->size, packet->status, packet->protocol);
}

// Value - Packet
struct bpf_map_def SEC("maps") packets_queue = {
        .type        = BPF_MAP_TYPE_QUEUE,
        .key_size    = 0,
        .value_size  = sizeof(struct Packet),
        .max_entries = queue_max_entries,
        .map_flags   = 0
};

// Key   - IPv4 address
// Value - Is blocked (bool flag)
struct bpf_map_def SEC("maps") blocked_ips_map = {
        .type        = BPF_MAP_TYPE_LRU_HASH,
        .key_size    = sizeof(uint),
        .value_size  = sizeof(bool),
        .max_entries = map_max_entries,
        .map_flags   = 0
};

SEC("xdp")
int bpf_xdp_handler(struct xdp_md *ctx) {
    int XDP_ACTION = XDP_PASS;

    void *data     = (void *)(long)ctx->data;
    void *data_end = (void *)(long)ctx->data_end;

    // Parse the ethernet header.
    struct ethhdr *eth = data;
    if ((void *)(eth + 1) > data_end) {
        return XDP_ACTION;
    }

    if (ntohs(eth->h_proto) != ETH_P_IP) {
        // The protocol is not IPv4.
        return XDP_ACTION;
    }

    // Parse the IP header.
    struct iphdr *iph = data + sizeof(struct ethhdr);
    if ((void *)(iph + 1) > data_end) {
        return XDP_ACTION;
    }

    // Check is ip blocked.
    uint ip = (uint)(iph->saddr);
    bool is_ip_blocked = bpf_map_lookup_elem(&blocked_ips_map, &ip);
    if (is_ip_blocked) {
        XDP_ACTION = XDP_DROP;
    }

    struct Packet packet;
    __builtin_memset(&packet, 0, sizeof(packet));

    packet.ip = ip;
    packet.size = data_end - data;
    packet.protocol = iph->protocol;
    packet.status = is_ip_blocked ? Dropped : Passed;

    // Add information about packet to queue.
    int error = bpf_map_push_elem(&packets_queue, &packet, BPF_ANY);
    if (error != 0) {
        bpf_printk("An error was occurred while adding to queue (code: %i)", error);
    }

    if (debug_trace) {
        trace_packet(&packet);
    }

    return XDP_ACTION;
}

char _license[] SEC("license") = "GPL";