# LOVEVERSE (A World Made for Two)

**Engine:** Unity 6 LTS (`6000.0.85f2`)  
**Rendering:** Universal Render Pipeline (URP)  
**Programming Language:** C# (C# 9.0+ / .NET Standard 2.1)  
**Platforms:** Android & Windows PC  
**Backend:** Node.js + TypeScript + Fastify  
**Database:** Firebase Authentication + Cloud Firestore  
**Multiplayer:** Photon Fusion 2 (Shared Mode)  
**Visual Style:** Premium Anime 3D  

---

## Overview
LOVEVERSE is a high-quality 3D romantic multiplayer open-world game designed for two people who want to spend time together in a peaceful, persistent virtual world. Players can customize their anime-style avatars, connect through private 2-player rooms via 6-character invitation codes, explore scenic environments, decorate a shared home, perform synchronized romantic interactions (holding hands, hugging, dancing, sitting, stargazing), capture persistent shared memories, and participate in cooperative activities.

---

## Project Structure
```text
LOVEVERSE/
├── Assets/
│   ├── _Project/
│   │   ├── Core/               # ServiceRegistry, EventBus, Bootstrapper, Configs
│   │   ├── Features/           # Locomotion, Camera, Characters, Romance, Networking, Home, Memories, World
│   │   ├── Art/                # Shaders, Materials, Textures
│   │   ├── Audio/              # Music, SFX, Ambience
│   │   ├── UI/                 # HUD, Dialogs, Virtual Joysticks
│   │   ├── Scenes/             # Bootstrap, MainMenu, LoveTown, OurHome
│   │   └── Tests/              # Automated NUnit test suite
│   └── ThirdParty/
├── Backend/                    # Fastify + TypeScript API & Firestore Repositories
├── Documentation/              # Complete GDD, Technical Architecture, API Specs
├── Tools/                      # Automation scripts & CI helpers
├── Packages/                   # Unity Package Manager manifest
└── ProjectSettings/            # Unity Project Settings
```

---

## Getting Started

### 1. Unity Client
1. Ensure **Unity 6 LTS** (`6000.0.85f2`) is installed via Unity Hub.
2. Open the project folder in Unity Editor.
3. Packages will resolve automatically via [`Packages/manifest.json`](Packages/manifest.json).
4. Open and run the `Bootstrap` scene located in `Assets/_Project/Scenes/`.

### 2. Backend Server
1. Navigate to `Backend/`:
   ```bash
   cd Backend
   npm install
   ```
2. Build and run tests:
   ```bash
   npm run build
   npm test
   ```
3. Start local development server:
   ```bash
   npm run dev
   ```
   The REST server listens on `http://localhost:3000`.

---

## Documentation
- [Game Design Document (GDD)](Documentation/GDD.md)
- [Technical Architecture](Documentation/TECHNICAL_ARCHITECTURE.md)
- [Multiplayer Specification](Documentation/NETWORKING_SPECIFICATION.md)
- [Backend Specification](Documentation/BACKEND_SPECIFICATION.md)
- [Database Schema](Documentation/DATABASE_SCHEMA.md)
- [API Reference](Documentation/API_REFERENCE.md)
- [Testing Strategy](Documentation/TESTING_STRATEGY.md)
- [Security Guide](Documentation/SECURITY_GUIDE.md)
- [Implementation Status](Documentation/IMPLEMENTATION_STATUS.md)
