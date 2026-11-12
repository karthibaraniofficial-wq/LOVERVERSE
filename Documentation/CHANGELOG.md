# LOVEVERSE - Changelog

## [1.0.0-MVP] - 2026-11-12
### Milestone: Production Architecture & Full 10-Part Engine Verified

- **Part 1 (Architecture & Setup):** Initialized Git repository, Unity 6 LTS directory hierarchy, `Packages/manifest.json`, 11 Assembly Definitions, and full documentation suite.
- **Part 2 (Core Foundation):** Implemented `ServiceRegistry`, `EventBus`, `GameBootstrapper`, `SceneService`, `Lg` structured logger with correlation IDs, and ScriptableObject configs (`MovementConfig`, `CameraConfig`, `RomanceConfig`, `WorldConfig`, `AudioConfig`, `NetworkConfig`).
- **Part 3 (Locomotion & Input):** Implemented cross-platform `IInputService` (Android virtual analog stick + Windows WASD/Mouse), `ThirdPersonController` (`CharacterController`, slope handling, ground check, gravity, acceleration), and `LocomotionStateMachine`.
- **Part 4 (Camera & Anime Shaders):** Implemented `CameraController` (Cinemachine orbit with sphere-cast collision avoidance), `PhotoModeController` (FOV, HUD hiding, viewport capture), and custom URP `AnimeToonCharacter.shader` (stepped cel shading, rim lighting, inverted hull outline).
- **Part 5 (Character Customization):** Implemented `CharacterAppearance`, `CustomizationItem`, item registries (Hairstyles, Eyes, Skins, Outfits), `CharacterCustomizationService`, and `CharacterVisualApplier`.
- **Part 6 (Romantic Interactions):** Implemented `InteractionType` (Wave, HighFive, HoldHands, Hug, Dance, SitTogether, Stargaze, GiveGift), `InteractionDetector` (distance + view angle), `InteractionRequestService` (consent workflow with 15s timer), `InteractionPermissionService` (cooldowns), and `PairedAnimationController` (anchor positioning).
- **Part 7 (Multiplayer Networking):** Implemented `INetworkService`, `RoomCodeGenerator` (cryptographic 6-character room codes with ambiguous character omission), `NetworkManager`, `NetworkCharacterState` (Hermite interpolation), and `SessionLifecycleService` (reconnection window).
- **Part 8 (Backend Services & Cloud DB):** Implemented Node.js + TypeScript Fastify server, Firebase Auth middleware, Cloud Firestore repository, REST endpoints (`/auth/session`, `/users/me`, `/rooms`, `/rooms/join`, `/inventory`, `/memories`, `/reports`, `/openapi.json`), and `firestore.rules`.
- **Part 9 (Home, Memories & Garden):** Implemented `HomePlacementService` (0.5m grid snapping & collision clearance), `MemoryService` (PNG disk cache and cloud metadata), `EnvironmentController` (24h sun rotation & ambient color blending), `WeatherController` (Clear, Rain, Fog), and `RomanticGardenBuilder`.
- **Part 10 (Integration & Verification):** Added 10 C# unit and simulation test suites, expanded backend automated test suite to 12 passing tests, and verified end-to-end integration pipeline via `Tools/verify_build.sh`.
