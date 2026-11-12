# LOVEVERSE - Changelog

## [1.1.0] - 2026-11-12
### Milestone: Four-Platform Universal Support (Android, iOS, Windows, Linux)

- **Platform Abstraction Layer (PAL):** Implemented `IPlatformService`, `PlatformDetector`, `PlatformStorageAdapter`, `PlatformPermissionService`, and `PlatformInputAdapter`.
- **Cross-Platform Storage:** Eliminated all hardcoded path separators; normalized persistent and cache storage paths across POSIX (Linux, Android, iOS) and Windows.
- **Mobile Safe Area & Notch Support:** Implemented `SafeAreaHandler` dynamically binding UI anchors to iPhone notches, Dynamic Island, and Android punch-hole cameras.
- **Unified Input Adapter:** Dynamically mounts mobile touch virtual joystick canvas on Android/iOS, and activates WASD + Mouse bindings on Windows/Linux with universal controller support.
- **Four-Platform Build Automation:** Implemented `PlatformBuildProfiles.cs` and `Tools/build_platforms.sh` supporting headless builds for Android (ARM64 APK/AAB), iOS (Xcode project), Windows (x64 standalone), and Linux (x64 standalone).
- **Scalable Graphics Presets:** Added `GraphicsQualityConfig` supporting Low, Medium, High, and Ultra tiers with dynamic resolution scale and platform-specific defaults.
- **Cross-Platform Test Suite:** Added `PlatformTests.cs` validating platform detection, storage path normalization, and quality preset scaling.

## [1.0.0-MVP] - 2026-11-12
### Milestone: Production Architecture & Full 10-Part Engine Verified

- Implemented Parts 1 through 10 of the Master Development Specification.
- Validated with 10 C# NUnit test suites and 12 Fastify / Vitest backend integration tests.
