@REM Deploys the API to Cloud Run
@REM Please setup the following variables before running

@echo off 
set gcp_project_id=sugi-dev-sandbox
set service_account=definitely-not-touhou@sugi-dev-sandbox.iam.gserviceaccount.com



CALL gcloud builds submit --tag eu.gcr.io/%gcp_project_id%/definitely-not-touhou-api

CALL gcloud run deploy definitely-not-touhou-api^
 --image eu.gcr.io/%gcp_project_id%/definitely-not-touhou-api^
 --region europe-west1 --allow-unauthenticated --platform managed^
 --service-account %service_account%
