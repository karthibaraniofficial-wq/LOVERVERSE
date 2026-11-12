# LOVEVERSE - Technical Architecture Specification

## 1. Engine & Runtime
- **Engine:** Unity 6 LTS (`6000.0.85f2`)
- **Render Pipeline:** Universal Render Pipeline (URP) with custom Cel-Shading Forward+ pass.
- **Scripting Runtime:** .NET Standard 2.1 / C# 9.0+
- **Input System:** New Unity Input System (`com.unity.inputsystem`) with platform adapters.
- **Camera:** Cinemachine (`com.unity.cinemachine`)
- **Text Rendering:** TextMeshPro (`com.unity.textmeshpro`)

## 2. Four-Platform Abstraction Layer
```
┌────────────────────────────────────────────────────────┐
│                   UI & Presentation                    │
│        (SafeAreaHandler, VirtualJoystick, Desktop HUD) │
├────────────────────────────────────────────────────────┤
│           Platform Abstraction Layer (PAL)             │
│   (IPlatformService, PlatformDetector, StorageAdapter, │
│    PlatformPermissionService, PlatformInputAdapter)    │
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

## 3. Platform Abstraction Components
1. **`IPlatformService` & `PlatformService`**: Master contract managing runtime target detection, form factor analysis, target framerate setting, fullscreen windowing, and permission delegation.
2. **`PlatformDetector`**: Evaluates platform compile flags and runtime platform type (`Android`, `iOS`, `Windows`, `Linux`) and categorizes form factors (`MobilePhone`, `Tablet`, `Desktop`).
3. **`PlatformStorageAdapter`**: Normalizes all file paths using OS directory separators, eliminating hardcoded backslashes and ensuring POSIX safety across Linux, Android, and iOS.
4. **`PlatformPermissionService`**: Handles mobile runtime permissions (Android camera/storage, iOS photo library) with immediate grant on desktop platforms.
5. **`PlatformInputAdapter`**: Selectively mounts virtual touch canvas or desktop keyboard/mouse listeners, and provides universal gamepad polling.
6. **`PlatformBuildProfiles`**: Automated headless build profiles for Android (APK/AAB), iOS (Xcode project), Windows (x64 standalone), and Linux (x64 standalone).

## 4. Assembly Definitions
Major systems are segregated into 12 independent assemblies:
- `Loveverse.Core`: Foundation services, platform abstraction layer, event bus, ScriptableObject configs.
- `Loveverse.Movement`: Locomotion controller, state machine, ground detector, input adapter.
- `Loveverse.Camera`: Cinemachine orbital controller, photo mode capture.
- `Loveverse.Characters`: Character definitions, customization service, mesh and material applier.
- `Loveverse.Romance`: Interaction detector, consent workflow, paired animation anchor controller.
- `Loveverse.Networking`: Photon Fusion 2 driver, room manager, replication state.
- `Loveverse.Home`: Furniture placement, grid snapping, layout serialization.
- `Loveverse.Memories`: Photo screenshot capture, PNG disk cache, cloud upload metadata.
- `Loveverse.World`: Environment controller, 24h day/night cycle, weather controller, garden builder.
- `Loveverse.UI`: Virtual joystick, action buttons, safe area adapter, HUD.
- `Loveverse.Editor`: Automated platform build profiles and editor tools.
- `Loveverse.Tests`: Automated NUnit test suite.
