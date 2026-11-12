# LOVEVERSE - Cross-Platform UI/UX Specification

## 1. Input Controls by Platform

### Mobile Platforms (Android & iOS)
- **Left Thumb:** Dynamic floating virtual analog joystick with deadzone and auto-centering.
- **Right Thumb:** Swipe touch delta for camera orbit; dedicated action buttons for Jump, Sprint toggle, and contextual Interact.
- **Notch & Safe Area:** `SafeAreaHandler` recalculates min/max normalized anchors against `Screen.safeArea` on iPhone notch/Dynamic Island, iPad home indicators, and Android hole-punch cameras.
- **Responsive Layout:** Adaptive font sizing and button margins scaling between 5.5" phones and 12.9" tablets.

### Desktop Platforms (Windows & Linux)
- `W/A/S/D`: Locomotion
- `Mouse Movement`: Orbit camera
- `Space`: Jump
- `Left Shift`: Sprint
- `E`: Contextual Interact
- `P`: Photo Mode
- `M`: World Map
- `I`: Inventory
- `B`: Emote Menu
- `Enter`: Chat Input
- `Esc`: Pause / Settings

### Gamepad / Controller Support (All Platforms)
- Left Stick: Locomotion
- Right Stick: Camera Look
- Button South (A / Cross): Jump
- Button East (B / Circle): Sprint / Cancel
- Button West (X / Square): Interact
- Start / Menu: Pause

## 2. Design Aesthetic
- Translucent frosted glass backgrounds (`rgba(255, 255, 255, 0.75)` or dark glass for night mode).
- Rounded pill buttons with gentle scale bounce on tap/click.
- Accessible contrast ratios, dynamic notch / safe-area handling, and localization-ready text tokens.
