from rabbitmq.events.base_event import BaseEvent


class DetectDdosAttacksEvent(BaseEvent):
    def __init__(self, id: str, device_id: str, date_from: str, date_to: str, created_at: str):
        super().__init__(id, created_at)
        self.device_id = device_id
        self.date_from = date_from
        self.date_to = date_to

    @staticmethod
    def parse_from_json(json):
        return DetectDdosAttacksEvent(json['id'], json['deviceId'], json['dateFrom'], json['dateTo'], json['createdAt'])
