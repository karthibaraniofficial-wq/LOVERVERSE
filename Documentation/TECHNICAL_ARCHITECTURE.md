# LOVEVERSE - Technical Architecture Specification

## 1. Engine & Runtime
- **Engine:** Unity 6 LTS (`6000.0.85f2`)
- **Render Pipeline:** Universal Render Pipeline (URP) with custom Cel-Shading Forward+ pass.
- **Scripting Runtime:** .NET Standard 2.1 / C# 9.0+
- **Input System:** New Unity Input System (`com.unity.inputsystem`)
- **Camera:** Cinemachine (`com.unity.cinemachine`)
- **Text Rendering:** TextMeshPro (`com.unity.textmeshpro`)

## 2. Layered Client Architecture
```
┌────────────────────────────────────────────────────────┐
│                   UI & Presentation                    │
├────────────────────────────────────────────────────────┤
│  Gameplay Systems (Locomotion, Romance, Home, Camera)  │
├────────────────────────────────────────────────────────┤
│           Networking Abstraction Layer                 │
│        (Photon Fusion 2 Shared Mode Driver)            │
├────────────────────────────────────────────────────────┤
│            Core Engine & Services                      │
│   (ServiceRegistry, EventBus, Config, SceneLoader)     │
└────────────────────────────────────────────────────────┘
```

## 3. Assembly Definitions
Major systems are segregated into independent assemblies to guarantee fast incremental compilation, enforce clean architectural boundaries, and eliminate cyclical dependencies:
- `Loveverse.Core`: Foundation services, service registry, event bus, configuration ScriptableObjects.
- `Loveverse.Gameplay`: Locomotion, camera controllers, environment day/night, home placement, memories.
- `Loveverse.Characters`: Character data models, customization service, mesh and material binding.
- `Loveverse.Romance`: Interaction detection, two-player consent state machine, paired animation alignment.
- `Loveverse.Networking`: Photon Fusion 2 abstraction, room management, replication contracts.
- `Loveverse.UI`: HUD, dialogs, virtual joysticks, settings, interaction prompts.
- `Loveverse.Tests`: Automated unit and integration test suite.
