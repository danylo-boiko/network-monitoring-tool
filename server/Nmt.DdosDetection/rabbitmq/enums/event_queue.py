from strenum import StrEnum


class EventQueue(StrEnum):
    DETECT_DDOS_ATTACKS = 'detect-ddos-attacks-queue'
    BLOCK_IP_ADDRESSES = 'block-ip-addresses-queue'
