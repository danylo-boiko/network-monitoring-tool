from configparser import ConfigParser
from mongo.readonly_client import MongoReadonlyClient


class DetectDdosAttacksEventConsumer:
    def __init__(self, config: ConfigParser):
        self.mongo_readonly_client = MongoReadonlyClient(config)

    def consume(self, channel, method, properties, body):
        pass
