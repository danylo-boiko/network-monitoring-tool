import json
import socket

from configparser import ConfigParser
from pika import BasicProperties
from dateutil import parser
from ipaddress import ip_address

from ml_detection.detection_service import DetectionService
from mongo.readonly_client import MongoReadonlyClient
from rabbitmq.events.detect_ddos_attacks_event import DetectDdosAttacksEvent
from rabbitmq.events.block_ip_addresses_event import BlockIpAddressesEvent
from rabbitmq.enums.event_exchanger import EventExchanger
from rabbitmq.enums.event_queue import EventQueue


class DetectDdosAttacksEventConsumer:
    def __init__(self, config: ConfigParser, ml_model_name: str):
        self.mongo_readonly_client = MongoReadonlyClient(config)
        self.detection_service = DetectionService(ml_model_name)
        self.rabbit_mq_host = config.get('RabbitMQ', 'host')
        self.protocols = self._get_socket_constants('IPPROTO_')

    def consume(self, channel, method, properties, body) -> None:
        detect_ddos_attacks_event_json = json.loads(body.decode('UTF-8'))['message']
        detect_ddos_attacks_event = DetectDdosAttacksEvent.parse_from_json(detect_ddos_attacks_event_json)

        passed_packets = self.mongo_readonly_client.packets_collection.aggregate_pandas_all([
            {'$match': {
                'DeviceId': detect_ddos_attacks_event.device_id,
                'CreatedAt': {
                    '$gte': parser.parse(detect_ddos_attacks_event.date_from),
                    '$lte': parser.parse(detect_ddos_attacks_event.date_to)
                },
                'Status': 1  # 'Passed' status
            }},
            {'$project': {
                '_id': 0,
                'Protocol': '$Protocol',
                'IP': '$Ip',
                'Size': '$Size'
            }}
        ])

        passed_packets['Protocol'] = passed_packets['Protocol'].apply(lambda protocol: self.protocols[protocol])
        passed_packets['IP'] = passed_packets['IP'].apply(lambda ip: ip_address(ip).__str__())

        ips_to_block = self.detection_service.detect_hostile_ips(passed_packets)

        if len(ips_to_block) == 0:
            return

        block_ip_addresses_event = BlockIpAddressesEvent(detect_ddos_attacks_event.device_id, ips_to_block)
        channel.basic_publish(exchange=EventExchanger.BLOCK_IP_ADDRESSES,
                              routing_key=EventQueue.BLOCK_IP_ADDRESSES,
                              properties=BasicProperties(message_id=block_ip_addresses_event.id),
                              body=block_ip_addresses_event.to_masstransit_format(self.rabbit_mq_host))

    def _get_socket_constants(self, prefix: str) -> dict[int, str]:
        return dict((getattr(socket, c), c.removeprefix(prefix)) for c in dir(socket) if c.startswith(prefix))
