# LOVEVERSE - Known Issues & Mitigation

1. **Resolved — Camera Assembly Dependency (CS0234 & CS0246):** `Loveverse.Camera.asmdef` was missing reference to `Loveverse.Movement`, causing `IInputService` resolution to fail during compilation. Resolved by adding `"Loveverse.Movement"` to `Loveverse.Camera.asmdef` and `Loveverse.Tests.asmdef`.
2. **Resolved — NetworkManager Departure Warning (CS0067):** `OnPlayerLeft` event contract was declared on `INetworkService` but lacked lifecycle invocation in `NetworkManager`. Implemented `HandlePlayerLeft(string playerId)` to invoke `OnPlayerLeft` and synchronize player count on remote partner departure.
3. **Resolved — VirtualJoystick Unused Field (CS0414):** `_handleRange` field in `VirtualJoystick` was assigned but unused in position math. Integrated `_handleRange` into `OnDrag` anchor calculation with fallback to background radius.
4. **Firebase Cross-Platform Transport:** Client utilizes direct authenticated REST HTTPS endpoints with Fastify backend to ensure seamless four-platform parity across Android, iOS, Windows, and Linux without native desktop SDK limitations.
