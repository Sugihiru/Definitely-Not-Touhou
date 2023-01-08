from enum import Enum

from pydantic import BaseModel


class Difficulty(str, Enum):
    NORMAL = "Normal"
    HARD = "Hard"
    LUNATIC = "Lunatic"


class SubmitScorePayload(BaseModel):
    score: int
    seconds_survived: float
    difficulty: Difficulty


class PublishScorePayload(BaseModel):
    username: str


class ScoreModel(BaseModel):
    author: str
    score: int
    # createdTime: float
    secondsSurvived: float
    tmpScoreId: str | None
    difficulty: Difficulty
