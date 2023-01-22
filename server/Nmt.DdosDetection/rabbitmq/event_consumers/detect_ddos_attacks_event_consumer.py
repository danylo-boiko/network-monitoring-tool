import json
from configparser import ConfigParser
from pika import BasicProperties
from mongo.readonly_client import MongoReadonlyClient
from rabbitmq.events.detect_ddos_attacks_event import DetectDdosAttacksEvent
from rabbitmq.events.block_ip_addresses_event import BlockIpAddressesEvent
from rabbitmq.enums.event_exchanger import EventExchanger
from rabbitmq.enums.event_queue import EventQueue
from dateutil import parser


class DetectDdosAttacksEventConsumer:
    def __init__(self, config: ConfigParser):
        self.mongo_readonly_client = MongoReadonlyClient(config)
        self.rabbit_mq_host = config.get('RabbitMQ', 'host')

    def consume(self, channel, method, properties, body):
        detect_ddos_attacks_event_json = json.loads(body.decode('UTF-8'))['message']
        detect_ddos_attacks_event = DetectDdosAttacksEvent.parse_from_json(detect_ddos_attacks_event_json)

        passed_packets = self.mongo_readonly_client.packets_collection.find({
            'DeviceId': detect_ddos_attacks_event.device_id,
            'CreatedAt': {
                '$gte': parser.parse(detect_ddos_attacks_event.date_from),
                '$lte': parser.parse(detect_ddos_attacks_event.date_to)
            },
            'Status': 1  # Passed status
        }, {
            '_id': 0,
            'Ip': 1,
            'Size': 1,
            'Protocol': 1,
            'CreatedAt': 1
        })

        print(list(passed_packets))

        # TODO get from ML
        ips_to_blocks = []

        if len(ips_to_blocks) == 0:
            return

        block_ip_addresses_event = BlockIpAddressesEvent(detect_ddos_attacks_event.device_id, ips_to_blocks)
        channel.basic_publish(exchange=EventExchanger.BLOCK_IP_ADDRESSES,
                              routing_key=EventQueue.BLOCK_IP_ADDRESSES,
                              properties=BasicProperties(message_id=block_ip_addresses_event.id),
                              body=block_ip_addresses_event.to_masstransit_format(self.rabbit_mq_host))
