# LOVEVERSE - Networking Specification

## 1. Multiplayer Architecture
- **Engine Provider:** Photon Fusion 2 (Shared Mode).
- **Session Types:**
  - **Private Rooms:** Max 2 players. Protected by 6-character alphanumeric invite codes.
  - **Public Hubs:** 16-player social sessions (Love Town Plaza).

## 2. Synchronization Strategy
- **Movement Replication:** Remote player position and rotation synced with Hermite cubic interpolation and dead-reckoning.
- **Locomotion States:** Enum flags synchronized via reliable network properties.
- **Interaction Replication:** RPC-coordinated paired animation triggers with server/host-validated anchors.

## 3. Disconnection & Reconnection
- Graceful leave RPC cancels active interactions.
- Unexpected network drop triggers 30-second reconnection window before session teardown.
