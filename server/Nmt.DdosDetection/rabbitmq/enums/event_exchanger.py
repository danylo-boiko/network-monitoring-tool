from strenum import StrEnum


class EventExchanger(StrEnum):
    BASE = 'Nmt.Domain.BusEvents:BaseEvent'
    DETECT_DDOS_ATTACKS = 'Nmt.Domain.BusEvents:DetectDdosAttacksEvent'
    BLOCK_IP_ADDRESSES = 'Nmt.Domain.BusEvents:BlockIpAddressesEvent'
