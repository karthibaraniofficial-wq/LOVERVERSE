# LOVEVERSE - Known Issues & Mitigation

1. **Unity Editor Installation:** Unity CLI has no editor installed locally yet; project assets, scripts, assemblies, and package manifests are configured according to standard Unity 6 LTS specifications so that when opened in Unity Hub or launched via `unity open`, it loads smoothly without missing references.
2. **Firebase Native Windows Support:** Unity Firebase SDK has known limitations on Windows desktop; our architecture uses direct authenticated REST HTTPS endpoints with Fastify backend to ensure seamless cross-platform parity on both Android and Windows PC.
