# LOVEVERSE - Four-Platform Performance & Quality Guide

## 1. Platform Performance Targets

| Platform | Target Architecture | Target Framerate | Memory Budget | Graphics API | Compression |
|---|---|---|---|---|---|
| **Android** | ARM64 | 30 FPS (Low-end) / 60 FPS (Mid/High) | < 1.2 GB | Vulkan / GLES3 | ASTC 6x6 |
| **iOS** | ARM64 (iPhone / iPad) | 60 FPS stable | < 1.4 GB | Metal | ASTC 5x5 / 6x6 |
| **Windows** | x86_64 (Windows 10/11) | 60 - 120+ FPS (1080p / 1440p) | < 3.5 GB | DX11 / DX12 / Vulkan | BC7 / DXT5 |
| **Linux** | x86_64 (Ubuntu / Kubuntu) | 60 - 120+ FPS (1080p / 1440p) | < 3.5 GB | Vulkan / OpenGL Core | BC7 / DXT5 |

## 2. Quality Presets Matrix

| Parameter | Low (Mobile Battery) | Medium (Default Mobile) | High (Default Desktop / High-End Tablet) | Ultra (Enthusiast Desktop) |
|---|---|---|---|---|
| **Render Scale** | 0.75x | 0.90x | 1.00x | 1.00x (Native High DPI) |
| **Shadow Distance** | 20m | 35m | 60m | 100m |
| **Soft Shadows** | Off | On (2 cascades) | On (4 cascades) | On (4 cascades + High Res) |
| **LOD Bias** | 0.70x | 1.00x | 1.50x | 2.00x |
| **Anisotropic Filtering** | Disabled | Per Texture | 2x Forced | 4x Forced |
| **Post-Processing** | Disabled | Bloom + Color Grading | Full URP Stack | Full URP Stack + High Depth of Field |

## 3. Platform Optimizations
- **Mobile (Android/iOS):** Thermal throttling detection and automatic render scale step-down; battery optimization cap at 60 FPS; strict primitive collider batching.
- **Desktop (Windows/Linux):** POSIX path safety on Linux; ALSA/PulseAudio/PipeWire compatibility; borderless fullscreen and native resolution scaling.
