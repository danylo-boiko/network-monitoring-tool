from strenum import StrEnum


class EventExchanger(StrEnum):
    BASE = 'EventBus.Messages.Events:IntegrationBaseEvent'
    DETECT_DDOS_ATTACKS = 'EventBus.Messages.Events:DetectDdosAttacksEvent'
    BLOCK_IP_ADDRESSES = 'EventBus.Messages.Events:BlockIpAddressesEvent'
