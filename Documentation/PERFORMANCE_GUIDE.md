# LOVEVERSE - Performance Guide

## Targets
- **Android Target:** 30 FPS minimum on low-end, 60 FPS on mid/high-end ARM64 devices. Memory budget: < 1.2 GB RAM.
- **Windows PC Target:** 60+ FPS at 1080p/1440p.

## Optimization Rules
- Static batching and GPU instancing on foliage and garden architecture.
- Texture compression: ASTC 6x6 on mobile, BC7 on PC.
- Audio source pooling and distance-attenuated 3D spatialization.
- CharacterController physics tick locked to 50 Hz.
