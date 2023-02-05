import os

from fastapi import FastAPI, Request, status
from fastapi.exceptions import RequestValidationError
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import JSONResponse

import scoring_challenge
import scoring_survival
from tags_metadata import tags_metadata

app = FastAPI(title="DefinitelyNotTouhouApi", openapi_tags=tags_metadata)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["Content-Type", "Authorization"],
)

# if os.environ.get("API_LOG_ENV", "development") == 'production':
#     setup_logging()
#     app.add_middleware(LoggingMiddleware)
# else:
#     logger.setLevel(logging.DEBUG)

# Add logs on HTTP 422 Unprocessable Entity
@app.exception_handler(RequestValidationError)
async def validation_exception_handler(request: Request, exc: RequestValidationError):
    exc_str = f"{exc}".replace("\n", " ").replace("   ", " ")
    # logging.error(f"Body: {exc.body} / Error: {exc_str}")
    content = {"status_code": 10422, "message": exc_str, "data": None}
    return JSONResponse(
        content=content, status_code=status.HTTP_422_UNPROCESSABLE_ENTITY
    )


# Setup routers
for router in (scoring_challenge.router, scoring_survival.router):
    app.include_router(router)


if __name__ == "__main__":
    import os

    import uvicorn

    uvicorn.run(app, port=int(os.environ.get("PORT", 8000)), host="0.0.0.0")
