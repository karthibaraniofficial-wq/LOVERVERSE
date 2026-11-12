# LOVEVERSE - Gameplay Systems Specification

## 1. Locomotion & Character Controller
- **Physics Driver:** Unity `CharacterController`.
- **States:** Idle, Walk, Run, Sprint, Jump, InAir, Fall, Sit, PairedAction.
- **Features:** Slope limit evaluation, step offset, gravity acceleration, ground snapping, camera-relative movement.
- **Configurations:** All values (walk speed, run speed, sprint speed, jump height, rotation speed) are externalized into `MovementConfig` ScriptableObjects.

## 2. Romantic Interaction Engine
- **Workflow:**
  1. Proximity trigger & line-of-sight check (< 2.5m).
  2. Initiator sends request via `InteractionRequestService`.
  3. Target receives prompt with a 15-second expiration timer.
  4. Upon acceptance, both characters align to `InteractionAnchor` relative offsets.
  5. Synchronized animation triggers with state locks.
  6. Either player can cancel at any time, returning both to Idle.
- **Interactions:** Wave, High-Five, Hold Hands, Hug, Dance, Sit Together, Stargaze, Give Gift.

## 3. Shared Home & Furniture System
- **Grid Placement:** 0.5m snapped grid with bounding box overlap tests.
- **State Serialization:** Item ID, world coordinates, rotation Euler, tint color, room zone.
- **Permissions:** Restricted to authenticated co-owners.

## 4. Shared Memories System
- In-game photo framing with FOV and filters.
- Capture to texture, local cache, and cloud upload.
- Metadata: Timestamp, location ID, captions, author ID.
