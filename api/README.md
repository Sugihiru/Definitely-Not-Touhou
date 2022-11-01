Deploying
=========


1. Create a Google Cloud Project, activate billing for this project

2. Initialize the Firebase Application from this project

3. Initialize the Firestore database

4. Create a service account, and grant it the following roles : `Datastore User`

5. Enable the following APIs : `cloudbuild.googleapis.com`, `artifactregistry.googleapis.com`, `run.googleapis.com` (or just follow this link to enable them : https://console.cloud.google.com/flows/enableapi?apiid=cloudbuild.googleapis.com,%20artifactregistry.googleapis.com,%20run.googleapis.com)




Running it in dev
=================

`uvicorn main:app --host 0.0.0.0 --port 8000 --reload`
