from configparser import ConfigParser
from pika import ConnectionParameters, BlockingConnection
from rabbitmq.enums.event_queue import EventQueue
from rabbitmq.enums.event_exchanger import EventExchanger
from rabbitmq.event_consumers.detect_ddos_attacks_event_consumer import DetectDdosAttacksEventConsumer


class RabbitMQConnector:
    def create_channel(self, config: ConfigParser):
        host = config.get('RabbitMQ', 'host')
        port = config.getint('RabbitMQ', 'port')

        connection_parameters = ConnectionParameters(host, port)
        connection = BlockingConnection(connection_parameters)

        channel = connection.channel()
        self._setup_channel_queues(channel, config)

        return channel

    def _setup_channel_queues(self, channel, config: ConfigParser):
        # consumers
        detect_ddos_attacks_event_consumer = DetectDdosAttacksEventConsumer(config)

        channel.queue_declare(queue=EventQueue.DETECT_DDOS_ATTACKS)
        channel.queue_bind(queue=EventQueue.DETECT_DDOS_ATTACKS, exchange=EventExchanger.DETECT_DDOS_ATTACKS)
        channel.basic_consume(queue=EventQueue.DETECT_DDOS_ATTACKS,
                              on_message_callback=detect_ddos_attacks_event_consumer.consume,
                              auto_ack=True)

        # providers
        channel.queue_declare(queue=EventQueue.BLOCK_IP_ADDRESSES, durable=True)

        return channel
