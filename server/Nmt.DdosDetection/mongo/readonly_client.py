import pymongoarrow.monkey

from pymongo import MongoClient
from configparser import ConfigParser


class MongoReadonlyClient:
    def __init__(self, config: ConfigParser):
        connection_string = config.get('MongoDB', 'connection_string')
        packets_collection_name = config.get('MongoDB', 'packets_collection_name')

        pymongoarrow.monkey.patch_all()

        mongo_client = MongoClient(connection_string)
        database = mongo_client.get_default_database()

        self.packets_collection = database.get_collection(packets_collection_name)
