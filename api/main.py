from typing import List, Optional

# import uvicorn
from fastapi import FastAPI, Path
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel

tags_metadata = [
    {
        "name": "scoring",
        "description": "Operations that affects the scoring system of the game",
    },
]

app = FastAPI(title="DefinitelyNotTouhouApi", openapi_tags=tags_metadata)

# app.add_middleware(
#     CORSMiddleware,
#     allow_origins=['*'],
#     allow_methods=["*"],
#     allow_headers=["Content-Type", "Authorization"],
# )

# Define setting of apps
# settings = Settings()

# if settings.environment == 'production':
#     setup_logging()
#     app.add_middleware(LoggingMiddleware)
# else:
#     logger.setLevel(logging.DEBUG)


class SubmitScorePayload(BaseModel):
    score: int
    seconds_survived: float


class PublishScorePayload(BaseModel):
    username: str


class ScoreModel(BaseModel):
    author: str
    score: int
    # createdTime: float
    secondsSurvived: float
    tmpScoreId: str | None


@app.post(
    "/publish-score/{score_id}",
    summary="Publish a score, associating an username with it",
    tags=["scoring"],
)
def publish_score(*, score_id: str = Path(), payload: PublishScorePayload):
    return f"publish-score {score_id} {payload.username}", 200


@app.post(
    "/submit-tmp-score",
    response_model=List[ScoreModel],
    summary="Submit a score without username that won't be displayed in the scoreboard",
    tags=["scoring"],
)
def submit_tmp_score(submitted_score: SubmitScorePayload):
    top_scores = get_top_scores()
    top_scores.append(
        ScoreModel(
            author="",
            score=submitted_score.score,
            secondsSurvived=submitted_score.seconds_survived,
            tmpScoreId="tmp-id",  # TODO: get from DB upload
        )
    )
    top_scores = sorted(top_scores, key=lambda x: x.secondsSurvived, reverse=True)[:10]
    return top_scores


@app.get("/get-top-scores", response_model=List[ScoreModel], tags=["scoring"])
def get_top_scores(username: Optional[str] = None, limit: Optional[int] = 10):
    return [
        ScoreModel(author="moi", score=10, secondsSurvived=3),
        ScoreModel(author="toi", score=1031, secondsSurvived=13),
        ScoreModel(author="lui", score=1110, secondsSurvived=33),
        ScoreModel(author="bob", score=1110, secondsSurvived=11),
        ScoreModel(author="bob", score=1110, secondsSurvived=40),
        ScoreModel(author="bob", score=1110, secondsSurvived=51),
        ScoreModel(author="bob", score=1110, secondsSurvived=13),
        ScoreModel(author="bob", score=1110, secondsSurvived=36),
        ScoreModel(author="bob", score=1110, secondsSurvived=11),
        ScoreModel(author="bob", score=1110, secondsSurvived=3),
    ]
    # return f"get_top_scores {username}", 200


if __name__ == "__main__":
    import os

    import uvicorn

    uvicorn.run(app, port=int(os.environ.get("PORT", 8000)), host="0.0.0.0")
