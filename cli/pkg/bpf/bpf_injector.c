// +build ignore

#include <stdbool.h>
#include <linux/bpf.h>
#include <bpf/bpf_helpers.h>
#include <netinet/in.h>
#include <linux/if_ether.h>
#include <linux/if_packet.h>
#include <linux/ip.h>
#include <linux/tcp.h>
#include <linux/udp.h>
#include <linux/icmp.h>

typedef unsigned int uint;

// Key   - Source IPv4 address
// Value - Count of packets
struct bpf_map_def SEC("maps") address_packets_map = {
        .type        = BPF_MAP_TYPE_LRU_HASH,
        .key_size    = sizeof(uint),
        .value_size  = sizeof(uint),
        .max_entries = 32,
        .map_flags   = 0
};

// Key   - IPv4 address
// Value - Is blocked
struct bpf_map_def SEC("maps") blocked_ips_map = {
        .type        = BPF_MAP_TYPE_LRU_HASH,
        .key_size    = sizeof(uint),
        .value_size  = sizeof(bool),
        .max_entries = 32,
        .map_flags   = 0
};

// 0 - Number of passed packets
// 1 - Number of dropped packets
// 2 - Total size of passed packets
// 3 - Number with TCP protocol
// 4 - Number with UDP protocol
// 5 - Number with ICMP protocol
// 6 - Number with Other protocol
struct bpf_map_def SEC("maps") stats_map = {
        .type        = BPF_MAP_TYPE_ARRAY,
        .key_size    = sizeof(uint),
        .value_size  = sizeof(uint),
        .max_entries = 7,
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
        int err = bpf_map_update_elem(&address_packets_map, &ip, &init_pkt_count, BPF_NOEXIST);
        if (err != 0) {
            bpf_printk("Failed to add key (%u) to ip map, err: %d", ip, err);
        }
    } else {
        __sync_fetch_and_add(pkt_count, 1);
    }

    bool is_ip_blocked = bpf_map_lookup_elem(&blocked_ips_map, &ip);
    int pkt_index = 0;
    if (is_ip_blocked) {
        XDP_ACTION = XDP_DROP;
        pkt_index = 1;
    }

    // Update number of Passed/Dropped packets
    uint *pass_drop_count = bpf_map_lookup_elem(&stats_map, &pkt_index);
    if (pass_drop_count){
        __sync_fetch_and_add(pass_drop_count, 1);
    } else{
        bpf_printk("Failed to update number of passed/dropped packets");
    }

    if (XDP_ACTION == XDP_DROP) {
        return XDP_ACTION;
    }

    // Update total size of passed packets
    int total_size_index = 2;
    uint *total_size = bpf_map_lookup_elem(&stats_map, &total_size_index);

    if (total_size) {
        __sync_fetch_and_add(total_size, data_end - data);
    } else {
        bpf_printk("Failed to update total size");
    }

    // Identify the protocol of the packet
    int protocol_index;
    if (iph->protocol == IPPROTO_TCP) {
        protocol_index = 3;
        struct tcphdr *tcp = (void*)iph + sizeof(*iph);
        if ((void *)(tcp + 1) > data_end){
            return XDP_ACTION;
        }
    } else if (iph->protocol == IPPROTO_UDP) {
        protocol_index = 4;
        struct udphdr *udp = (void*)iph + sizeof(*iph);
        if ((void *)(udp + 1) > data_end){
            return XDP_ACTION;
        }
    } else if (iph->protocol == IPPROTO_ICMP) {
        struct icmphdr *icmp = (void*)iph + sizeof(*iph);
        if ((void *)(icmp + 1) > data_end){
            return XDP_ACTION;
        }
        protocol_index = 5;
    } else {
        protocol_index = 6;
    }

    // Update counter of protocol
    uint *protocol_count = bpf_map_lookup_elem(&stats_map, &protocol_index);
    if (protocol_count) {
        __sync_fetch_and_add(protocol_count, 1);
    } else {
        bpf_printk("Failed to update protocol counter");
    }

    return XDP_ACTION;
}

char _license[] SEC("license") = "GPL";