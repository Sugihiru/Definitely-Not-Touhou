import os
from typing import List, Optional

import firebase_admin
import google.cloud.firestore_v1 as fsv1
from fastapi import FastAPI, HTTPException, Path
from fastapi.middleware.cors import CORSMiddleware
from firebase_admin import credentials, firestore

from models import PublishScorePayload, ScoreModel, SubmitScorePayload

ROOT_COLLECTION = os.environ.get("DEFINITELY_NOT_TOUHOU_FSDB_BASE_COLLECTION", "")
SCORE_COLLECTION_PATH = f"{ROOT_COLLECTION}/scores" if ROOT_COLLECTION else "scores"
PENDING_SCORE_COLLECTION_PATH = (
    f"{ROOT_COLLECTION}/pendingScoreUploads"
    if ROOT_COLLECTION
    else "pendingScoreUploads"
)

cred = credentials.ApplicationDefault()
firebase_admin.initialize_app(
    cred, {"projectId": os.environ.get("GCP_PROJECT", "sugi-dev-sandbox")}
)
db: fsv1.Client = firestore.client()

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


@app.post(
    "/publish-score/{score_id}",
    summary="Publish a score, associating an username with it",
    tags=["scoring"],
)
def publish_score(*, score_id: str = Path(), payload: PublishScorePayload):
    tmp_score_doc = (
        db.collection(PENDING_SCORE_COLLECTION_PATH).document(score_id).get()
    )

    tmp_score_data = tmp_score_doc.to_dict()

    if not tmp_score_data:
        raise HTTPException(status_code=404, detail="Score not found")

    fs_transaction = db.transaction()
    fs_transaction.delete(
        db.collection(PENDING_SCORE_COLLECTION_PATH).document(score_id)
    )
    fs_transaction.create(
        db.collection(SCORE_COLLECTION_PATH).document(score_id),
        tmp_score_data
        | {
            "createdTime": firestore.firestore.SERVER_TIMESTAMP,
            "author": payload.username,
        },
    )
    fs_transaction.commit()
    return 200


@app.post(
    "/submit-tmp-score",
    response_model=List[ScoreModel],
    summary="Submit a score without username that won't be displayed in the scoreboard",
    tags=["scoring"],
)
def submit_tmp_score(submitted_score: SubmitScorePayload):
    limit = 10
    top_scores = get_top_scores(difficulty=submitted_score.difficulty, limit=limit)

    tmp_doc_id = db.collection(PENDING_SCORE_COLLECTION_PATH).document().id
    score = ScoreModel(
        author="",
        score=submitted_score.score,
        secondsSurvived=submitted_score.seconds_survived,
        tmpScoreId=tmp_doc_id,
        difficulty=submitted_score.difficulty,
    )
    top_scores.append(score)
    top_scores = sorted(top_scores, key=lambda x: x.secondsSurvived, reverse=True)[
        :limit
    ]
    if score in top_scores:
        db.collection(PENDING_SCORE_COLLECTION_PATH).document(tmp_doc_id).set(
            {
                x: y
                for x, y in score.dict().items()
                if x in ("score", "secondsSurvived", "difficulty")
            }
            | {"createdTime": firestore.firestore.SERVER_TIMESTAMP}
        )
    return top_scores


@app.get("/get-top-scores", response_model=List[ScoreModel], tags=["scoring"])
def get_top_scores(difficulty: str, username: Optional[str] = None, limit: int = 10):
    query = db.collection(SCORE_COLLECTION_PATH).where("difficulty", "==", difficulty)
    if username:
        query = query.where("author", "==", username)
    docs = query.order_by("secondsSurvived", direction="DESCENDING").limit(limit).get()

    return [ScoreModel(**data) for data in (doc.to_dict() for doc in docs) if data]


if __name__ == "__main__":
    import os

    import uvicorn

    uvicorn.run(app, port=int(os.environ.get("PORT", 8000)), host="0.0.0.0")
