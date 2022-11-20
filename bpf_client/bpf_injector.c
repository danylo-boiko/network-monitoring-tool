#include <linux/bpf.h>
#include <bpf/bpf_helpers.h>

SEC("bpf_xdp")
int bpf_xdp_handler(struct xdp_md *ctx) {
    void *data = (void *)(long)ctx->data;
    void *data_end = (void *)(long)ctx->data_end;

    int packet_size = data_end - data;

    bpf_printk("Packet size: %d", packet_size);

    return XDP_PASS;
}

char _license[] SEC("license") = "GPL";
