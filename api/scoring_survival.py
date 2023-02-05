import os
from typing import List, Optional

from fastapi import APIRouter, HTTPException, Path
from firebase_admin import firestore

from firestore_database import db
from models import (
    Difficulty,
    PublishScorePayload,
    SurvivalScoreModel,
    SurvivalSubmitScorePayload,
)

ROOT_COLLECTION = os.environ.get("DEFINITELY_NOT_TOUHOU_FSDB_BASE_COLLECTION", "")
SCORE_COLLECTION_PATH = (
    f"{ROOT_COLLECTION}/survivalScores" if ROOT_COLLECTION else "survivalScores"
)
PENDING_SCORE_COLLECTION_PATH = (
    f"{ROOT_COLLECTION}/pendingScoreUploads"
    if ROOT_COLLECTION
    else "pendingScoreUploads"
)

router = APIRouter(
    prefix="/survival",
    tags=["Survival"],
)


@router.post(
    "/publish-score/{score_id}",
    summary="Publish a score, associating an username with it",
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


@router.post(
    "/submit-tmp-score",
    response_model=List[SurvivalScoreModel],
    summary="Submit a score without username that won't be displayed in the scoreboard",
)
def submit_tmp_score(submitted_score: SurvivalSubmitScorePayload):
    limit = 10
    top_scores = get_top_scores(
        difficulty=submitted_score.difficulty,
        limit=limit,
    )

    tmp_doc_id = db.collection(PENDING_SCORE_COLLECTION_PATH).document().id
    score = SurvivalScoreModel(
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


@router.get(
    "/get-top-scores",
    response_model=List[SurvivalScoreModel],
)
def get_top_scores(
    difficulty: Difficulty,
    username: Optional[str] = None,
    limit: int = 10,
):
    query = db.collection(SCORE_COLLECTION_PATH).where("difficulty", "==", difficulty)
    if username:
        query = query.where("author", "==", username)
    docs = query.order_by("secondsSurvived", direction="DESCENDING").limit(limit).get()

    return [
        SurvivalScoreModel(**data) for data in (doc.to_dict() for doc in docs) if data
    ]
