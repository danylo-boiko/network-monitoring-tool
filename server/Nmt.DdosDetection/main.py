import sys
import os
from configparser import ConfigParser
from rabbitmq.connector import RabbitMQConnector


def main():
    config = ConfigParser()
    config.read('config.dev.ini')

    connector = RabbitMQConnector()
    channel = connector.create_channel(config)

    print('[*] Waiting for messages. To exit press CTRL+C.')
    channel.start_consuming()


if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt:
        print('Interrupted')
        try:
            sys.exit(0)
        except SystemExit:
            os._exit(0)
