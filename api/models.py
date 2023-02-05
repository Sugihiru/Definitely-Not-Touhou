from enum import Enum

from pydantic import BaseModel


class Difficulty(str, Enum):
    NORMAL = "Normal"
    HARD = "Hard"
    LUNATIC = "Lunatic"


class GameMode(str, Enum):
    SURVIVAL = "Survival"
    CHALLENGE = "Challenge"


class PublishScorePayload(BaseModel):
    username: str


class SurvivalScoreModel(BaseModel):
    author: str
    score: int
    # createdTime: float
    secondsSurvived: float
    tmpScoreId: str | None
    difficulty: Difficulty


class SurvivalSubmitScorePayload(BaseModel):
    score: int
    seconds_survived: float
    difficulty: Difficulty


class ChallengeScoreModel(BaseModel):
    author: str
    # createdTime: float
    secondsElapsed: float
    tmpScoreId: str | None


class ChallengeSubmitScorePayload(BaseModel):
    score: int
    seconds_elapsed: float
