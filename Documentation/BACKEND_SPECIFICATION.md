# LOVEVERSE - Backend Specification

## 1. Stack & Architecture
- **Runtime:** Node.js (v22 LTS)
- **Language:** TypeScript
- **Framework:** Fastify (high performance, low overhead)
- **Authentication:** Firebase Admin SDK (token validation)
- **Database:** Google Cloud Firestore

## 2. API Endpoints
- `POST /api/v1/auth/session`: Exchange Firebase ID token for validated session.
- `GET /api/v1/users/me`: Fetch authenticated profile & customization state.
- `PATCH /api/v1/users/me`: Update display name and visual preset.
- `POST /api/v1/rooms`: Create a 2-player private room and generate invite code.
- `POST /api/v1/rooms/join`: Validate code and join room.
- `DELETE /api/v1/rooms/:id`: Terminate private room session.
- `GET /api/v1/worlds/:id`: Fetch persistent shared world data (furniture layout, weather).
- `PATCH /api/v1/worlds/:id`: Save persistent shared world changes.
- `GET /api/v1/memories/:worldId`: List photo album memories.
- `POST /api/v1/memories`: Store new photo metadata.
- `POST /api/v1/reports`: Submit player moderation report.
