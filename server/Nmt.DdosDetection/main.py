import sys
import os
import argparse

from configparser import ConfigParser

from ml_detection.detection_service import DetectionService
from rabbitmq.connector import RabbitMQConnector


def main() -> None:
    config = ConfigParser()
    config.read('config.dev.ini')

    parser = argparse.ArgumentParser()

    parser.add_argument('-train', action=argparse.BooleanOptionalAction, help='Run model training flag')
    parser.add_argument('-model', action='store', type=str, help='Name of model')

    args = parser.parse_args()

    ml_model_name = 'default' if args.model is None else args.model

    if args.train:
        detection_service = DetectionService(ml_model_name)
        detection_service.train_model()

    connector = RabbitMQConnector(config, ml_model_name)
    channel = connector.create_channel()

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
