// +build ignore

#include <linux/bpf.h>
#include <bpf/bpf_helpers.h>
#include <netinet/in.h>
#include <linux/if_ether.h>
#include <linux/if_packet.h>
#include <linux/ip.h>

typedef unsigned int uint;

// Key   - Source IPv4 address
// Value - Packet count
struct bpf_map_def SEC("maps") address_packets_map = {
        .type        = BPF_MAP_TYPE_LRU_HASH,
        .key_size    = sizeof(uint),
        .value_size  = sizeof(uint),
        .max_entries = 16,
        .map_flags   = 0
};

SEC("xdp")
int bpf_xdp_handler(struct xdp_md *ctx) {
    int XDP_ACTION = XDP_PASS;

    void *data     = (void *)(long)ctx->data;
    void *data_end = (void *)(long)ctx->data_end;

    // Parse the ethernet header
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

    uint ip = (uint)(iph->saddr);
    uint *pkt_count = bpf_map_lookup_elem(&address_packets_map, &ip);

    if (!pkt_count) {
        uint init_pkt_count = 1;
        bpf_map_update_elem(&address_packets_map, &ip, &init_pkt_count, BPF_NOEXIST);
    } else {
        __sync_fetch_and_add(pkt_count, 1);
        bpf_printk("Packet size: %d", *pkt_count);
    }

    return XDP_ACTION;
}

char _license[] SEC("license") = "GPL";