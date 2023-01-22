import json
from rabbitmq.events.base_event import BaseEvent
from rabbitmq.enums.event_exchanger import EventExchanger


class BlockIpAddressesEvent(BaseEvent):
    def __init__(self, device_id: str, ips: list[int]):
        super().__init__()
        self.device_id = device_id
        self.ips = ips

    def to_masstransit_format(self, host: str) -> str:
        return json.dumps({
            'messageId': self.id,
            'conversationId': self.id,
            'destinationAddress': f'rabbitmq://{host}/{EventExchanger.BLOCK_IP_ADDRESSES}',
            'messageType': [
                f'urn:message:{EventExchanger.BLOCK_IP_ADDRESSES}',
                f'urn:message:{EventExchanger.BASE}'
            ],
            'message': {
                'id': self.id,
                'deviceId': self.device_id,
                'ips': self.ips,
                'createdAt': self.created_at
            }
        })
