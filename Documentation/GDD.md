# LOVEVERSE - Game Design Document (GDD)
**Project Codename:** LOVEVERSE  
**Tagline:** A World Made for Two.  
**Document Version:** 1.1 (Four-Platform Universal Edition)  
**Target Engine:** Unity 6 LTS (`6000.0.85f2`) | Universal Render Pipeline (URP)  
**Language:** C# | Node.js (TypeScript)  
**Supported Platforms:** Android (ARM64), iOS (iPhone & iPad), Windows (10/11 64-bit), Linux (Ubuntu/Kubuntu & compatible 64-bit distributions)  

## Executive Summary
LOVEVERSE is a high-quality 3D romantic multiplayer open-world game where two people can spend time together in a persistent, beautiful anime-styled virtual world. Players customize characters, connect via private rooms, explore scenic regions, decorate a shared home, perform synchronized romantic interactions (holding hands, hugging, dancing, stargazing), capture persistent photo memories, and participate in peaceful activities.

## Cross-Platform Ecosystem
All four target platforms share a unified game architecture, common player accounts, synchronized private rooms, and cross-play multiplayer:
- **Mobile (Android & iOS):** Responsive touch controls, dynamic floating virtual joystick, safe-area / notch adaptation, adaptive resolution, battery and thermal scaling.
- **Desktop (Windows & Linux):** Precision WASD + mouse camera, space jump, shift sprint, hotkeys (E, B, I, M, Esc), native fullscreen/windowed resolution switching, and full Ubuntu/Kubuntu compatibility.
- **Multiplayer Cross-Play:** Universal 6-character room codes allow seamless sessions between Android, iOS, Windows, and Linux.
