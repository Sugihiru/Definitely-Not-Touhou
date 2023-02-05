import os

import firebase_admin
import google.cloud.firestore_v1 as fsv1
from firebase_admin import credentials, firestore

cred = credentials.ApplicationDefault()
firebase_admin.initialize_app(
    cred, {"projectId": os.environ.get("GCP_PROJECT", "sugi-dev-sandbox")}
)
db: fsv1.Client = firestore.client()
