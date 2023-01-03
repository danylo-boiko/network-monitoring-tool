import argparse
from random import randrange
from scapy.all import sendp
from scapy.layers.inet import *


DEFAULT_SENDING_INTERVAL_SECONDS = 2
DEFAULT_COUNT_OF_PACKETS = 5


def get_random_ip() -> str:
    return '{}.{}.{}.{}'.format(randrange(1, 255), randrange(1, 255), randrange(1, 255), randrange(1, 255))


def send_packets(destination: str, iface: str, source_ip: str, sleep_seconds: int, count_of_packets: int) -> None:
    for _ in range(count_of_packets):
        port = randrange(1, 1000)
        protocol_id = randrange(0, 4)

        if protocol_id == 0:
            sendp(Ether()/IP(src=source_ip, dst=destination, ttl=(1, 1))/TCP(dport=port, flags='S'), iface=iface)
            protocol = 'TCP'
        elif protocol_id == 1:
            sendp(Ether()/IP(src=source_ip, dst=destination, ttl=(1, 1))/UDP(dport=port), iface=iface)
            protocol = 'UDP'
        elif protocol_id == 2:
            sendp(Ether()/IP(src=source_ip, dst=destination, ttl=(1, 1))/ICMP(), iface=iface)
            protocol = 'ICMP'
        else:
            sendp(Ether()/IP(src=source_ip, dst=destination, ttl=(1, 1)), iface=iface)
            protocol = 'Other'

        print('Source IP: {}\tProtocol: {}'.format(source_ip, protocol))

        time.sleep(sleep_seconds)


def main() -> None:
    parser = argparse.ArgumentParser()

    parser.add_argument('dst', type=str, help='destination ip address')
    parser.add_argument('iface', type=str, help='destination ip address interface')
    parser.add_argument('-src', action='store', type=str, help='source ip address')
    parser.add_argument('-slp', action='store', type=int, help='sleep interval in seconds between sending packets')
    parser.add_argument('-cnt', action='store', type=int, help='count of packets')

    args = parser.parse_args()

    if args.src is None:
        args.src = get_random_ip()

    if args.slp is None:
        args.slp = DEFAULT_SENDING_INTERVAL_SECONDS

    if args.cnt is None:
        args.cnt = DEFAULT_COUNT_OF_PACKETS

    send_packets(args.dst, args.iface, args.src, args.slp, args.cnt)


if __name__ == '__main__':
    main()
