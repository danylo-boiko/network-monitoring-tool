from uuid import uuid4
from datetime import datetime


class BaseEvent:
    def __init__(self, id=str(uuid4()), created_at=datetime.utcnow().isoformat()):
        self.id = id
        self.created_at = created_at
