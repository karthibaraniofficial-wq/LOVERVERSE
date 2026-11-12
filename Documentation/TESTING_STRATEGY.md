# LOVEVERSE - Testing Strategy & Verification Report

## 1. Test Levels
- **Unit Tests (C# NUnit in `Assets/_Project/Tests/`):**
  - `ServiceRegistryTests`: Service registration, resolution, lifecycle cleanup.
  - `EventBusTests`: Typed pub/sub event dispatching and unsubscription.
  - `LocomotionTests`: Character jump velocity calculation and diagonal input clamping.
  - `CameraTests`: Pitch clamping, distance damping, and target pivot offsets.
  - `CharacterCustomizationTests`: Independent deep-cloning and HTML hex color parsing.
  - `RoomCodeTests`: 6-character cryptographic length, ambiguous character omission, format validator.
  - `RomanceValidationTests`: Proximity range, line of sight, angle verification, consent expiration.
  - `HomePlacementTests`: 0.5m grid snapping precision, layout JSON serialization/deserialization.
  - `MemoryMetadataTests`: Memory data model initialization and JSON conversion.
  - `EndToEndSimulationTests`: Complete 14-step gameplay and lifecycle loop simulation (Boot -> Auth -> Character -> Room -> Join -> Sync -> Romance -> Home -> Memory -> Save -> Disconnect -> Reconnect).

- **Integration Tests (Fastify / Vitest in `Backend/tests/api.test.ts`):**
  - `GET /health`: Healthcheck status verification.
  - `GET /api/v1/openapi.json`: OpenAPI 3.0.3 schema generation.
  - Auth Token Middleware: 401 Unauthorized rejection for missing/invalid bearer tokens.
  - `POST /api/v1/auth/session`: User profile creation and lastLogin updates.
  - `POST /api/v1/rooms`: 6-character private room creation.
  - `POST /api/v1/rooms/join`: Room joining and active status transitions.
  - `POST /api/v1/rooms/join`: 404 rejection for non-existent room codes.
  - `GET & POST /api/v1/inventory/transactions`: Server-validated currency deduction and item unlocking.
  - `POST & GET /api/v1/memories`: Screenshot metadata persistence and world album queries.
  - `PATCH /api/v1/worlds/:id`: Persistent world weather and time-of-day synchronization.
  - `POST /api/v1/reports`: Player moderation report dispatch.
  - `DELETE /api/v1/users/me`: Account deletion and data removal compliance.

## 2. Automated Test Execution
Run the verification script from project root:
```bash
./Tools/verify_build.sh
```
All 12 backend integration tests and 10 C# test suites are verified.
