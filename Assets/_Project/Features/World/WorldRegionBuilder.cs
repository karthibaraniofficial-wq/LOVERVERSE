using System.Threading.Tasks;
using UnityEngine;
using Loveverse.Core;
using Loveverse.Movement;
using Loveverse.Characters;
using Loveverse.Romance;
using Loveverse.Networking;
using Loveverse.Camera;
using Loveverse.Home;

namespace Loveverse.World
{
    public enum WorldRegionType
    {
        LoveTown,
        OurHome,
        SunsetBeach,
        DreamForest,
        SkyMountain,
        StarlightCity
    }

    /// <summary>
    /// Master world region environment constructor.
    /// Sets up lighting, atmosphere, procedural environment geometry, local player avatar,
    /// orbiting camera controller, partner character with interaction detection, and navigation HUD.
    /// Supports direct play from any world scene with auto-bootstrap fallback.
    /// </summary>
    public sealed class WorldRegionBuilder : MonoBehaviour
    {
        [Header("Region Identity")]
        [SerializeField] private WorldRegionType _regionType = WorldRegionType.LoveTown;
        [SerializeField] private string _regionDisplayName = "Love Town";

        [Header("Lighting & Sky")]
        [SerializeField] private Color _skyColor = new Color(0.95f, 0.90f, 0.95f);
        [SerializeField] private Color _sunColor = new Color(1.0f, 0.95f, 0.85f);
        [SerializeField] private float _sunIntensity = 1.2f;
        [SerializeField] private Vector3 _sunRotation = new Vector3(45f, -30f, 0f);

        private GameObject _localPlayer;
        private GameObject _partnerCharacter;
        private GameObject _mainCamera;

        private async void Awake()
        {
            // Auto-fallback bootstrap if developer launched directly into this scene
            await EnsureServicesInitializedAsync();
            BuildEnvironment();
            SpawnLocalPlayer();
            SpawnPartner();
            SetupCamera();
        }

        private async Task EnsureServicesInitializedAsync()
        {
            if (!ServiceRegistry.Instance.TryResolve<IPlatformService>(out _))
            {
                var p = gameObject.AddComponent<PlatformService>();
                await p.InitializeAsync();
                ServiceRegistry.Instance.Register<IPlatformService>(p);
            }

            if (!ServiceRegistry.Instance.TryResolve<ISceneService>(out _))
            {
                var s = gameObject.AddComponent<SceneService>();
                await s.InitializeAsync();
                ServiceRegistry.Instance.Register<ISceneService>(s);
            }

            if (!ServiceRegistry.Instance.TryResolve<ICharacterCustomizationService>(out _))
            {
                var c = new CharacterCustomizationService();
                await c.InitializeAsync();
                ServiceRegistry.Instance.Register<ICharacterCustomizationService>(c);
            }
        }

        private void BuildEnvironment()
        {
            Lg.Info($"Building World Region: {_regionDisplayName} ({_regionType})");

            // 1. Sun Directional Light
            var lightObj = GameObject.Find("Directional Light");
            if (lightObj == null)
            {
                lightObj = new GameObject("Directional Light");
                var l = lightObj.AddComponent<Light>();
                l.type = LightType.Directional;
            }
            var sun = lightObj.GetComponent<Light>();
            sun.color = _sunColor;
            sun.intensity = _sunIntensity;
            lightObj.transform.rotation = Quaternion.Euler(_sunRotation);

            // 2. Ground / Terrain Construction
            var ground = GameObject.Find("Environment_Ground");
            if (ground == null)
            {
                ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
                ground.name = "Environment_Ground";
                ground.transform.position = Vector3.zero;
                ground.transform.localScale = new Vector3(8f, 1f, 8f); // 80x80 meters

                var rend = ground.GetComponent<Renderer>();
                if (rend != null)
                {
                    Color groundColor = _regionType switch
                    {
                        WorldRegionType.OurHome => new Color(0.72f, 0.53f, 0.35f), // Warm wood
                        WorldRegionType.SunsetBeach => new Color(0.94f, 0.87f, 0.65f), // Golden sand
                        WorldRegionType.DreamForest => new Color(0.25f, 0.45f, 0.28f), // Deep mossy forest
                        WorldRegionType.SkyMountain => new Color(0.55f, 0.60f, 0.65f), // Alpine stone
                        WorldRegionType.StarlightCity => new Color(0.18f, 0.20f, 0.28f), // Dark rooftop tiles
                        _ => new Color(0.48f, 0.68f, 0.42f) // Town park grass
                    };
                    rend.material.color = groundColor;
                }
            }

            // 3. Region Landmark Elements
            BuildLandmarks();
        }

        private void BuildLandmarks()
        {
            var landmarks = new GameObject($"Landmarks_{_regionType}");
            landmarks.transform.position = Vector3.zero;

            switch (_regionType)
            {
                case WorldRegionType.LoveTown:
                    // Central Plaza Fountain & Garden
                    var fountain = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    fountain.name = "Plaza_Fountain";
                    fountain.transform.position = new Vector3(0f, 0.5f, 8f);
                    fountain.transform.localScale = new Vector3(5f, 1f, 5f);
                    fountain.GetComponent<Renderer>().material.color = new Color(0.85f, 0.90f, 0.95f);
                    fountain.transform.SetParent(landmarks.transform);
                    break;

                case WorldRegionType.OurHome:
                    // Cottage Walls & Roof Structure
                    var homeMgr = gameObject.AddComponent<HomeManager>();
                    var homePlacement = gameObject.AddComponent<HomePlacementService>();
                    break;

                case WorldRegionType.SunsetBeach:
                    // Pier Stargazing Platform
                    var pier = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    pier.name = "Romantic_Pier";
                    pier.transform.position = new Vector3(0f, 0.2f, 15f);
                    pier.transform.localScale = new Vector3(4f, 0.4f, 20f);
                    pier.GetComponent<Renderer>().material.color = new Color(0.55f, 0.38f, 0.25f);
                    pier.transform.SetParent(landmarks.transform);
                    break;

                case WorldRegionType.DreamForest:
                    // Gazebo & Blossom Trunks
                    for (int i = 0; i < 6; i++)
                    {
                        float angle = i * 60f * Mathf.Deg2Rad;
                        var tree = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        tree.name = $"Blossom_Tree_{i + 1}";
                        tree.transform.position = new Vector3(Mathf.Cos(angle) * 14f, 2.5f, Mathf.Sin(angle) * 14f);
                        tree.transform.localScale = new Vector3(1.2f, 5f, 1.2f);
                        tree.GetComponent<Renderer>().material.color = new Color(1f, 0.75f, 0.85f); // Pink blossom
                        tree.transform.SetParent(landmarks.transform);
                    }
                    break;

                case WorldRegionType.SkyMountain:
                    // Summit Observation Railing
                    var deck = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    deck.name = "Observation_Deck";
                    deck.transform.position = new Vector3(0f, 0.3f, 10f);
                    deck.transform.localScale = new Vector3(12f, 0.6f, 12f);
                    deck.GetComponent<Renderer>().material.color = new Color(0.6f, 0.5f, 0.4f);
                    deck.transform.SetParent(landmarks.transform);
                    break;

                case WorldRegionType.StarlightCity:
                    // Rooftop Candlelit Couple Table
                    var table = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    table.name = "Romantic_Dinner_Table";
                    table.transform.position = new Vector3(0f, 0.75f, 5f);
                    table.transform.localScale = new Vector3(2.5f, 0.8f, 2.5f);
                    table.GetComponent<Renderer>().material.color = new Color(0.95f, 0.95f, 1f);
                    table.transform.SetParent(landmarks.transform);
                    break;
            }
        }

        private void SpawnLocalPlayer()
        {
            _localPlayer = GameObject.Find("Player_Local");
            if (_localPlayer == null)
            {
                _localPlayer = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                _localPlayer.name = "Player_Local";
                _localPlayer.transform.position = new Vector3(0f, 1f, 0f);

                // Add character locomotion components
                var charController = _localPlayer.AddComponent<CharacterController>();
                charController.center = new Vector3(0f, 0f, 0f);
                charController.height = 1.8f;
                charController.radius = 0.35f;

                var inputReader = _localPlayer.AddComponent<InputReader>();
                _localPlayer.AddComponent<PlatformInputAdapter>();
                var tpc = _localPlayer.AddComponent<ThirdPersonController>();
                _localPlayer.AddComponent<CharacterRotationController>();
                _localPlayer.AddComponent<GroundDetector>();

                var visual = _localPlayer.AddComponent<CharacterVisualApplier>();
                if (ServiceRegistry.Instance.TryResolve<ICharacterCustomizationService>(out var custom))
                {
                    visual.ApplyAppearance(custom.CurrentAppearance);
                }

                // Romance interaction coordinator
                _localPlayer.AddComponent<InteractionDetector>();
                var reqService = _localPlayer.AddComponent<InteractionRequestService>();
                _localPlayer.AddComponent<InteractionCoordinator>();
            }
        }

        private void SpawnPartner()
        {
            _partnerCharacter = GameObject.Find("Player_Partner");
            if (_partnerCharacter == null)
            {
                _partnerCharacter = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                _partnerCharacter.name = "Player_Partner";
                _partnerCharacter.transform.position = new Vector3(1.8f, 1f, 0.5f);
                _partnerCharacter.transform.rotation = Quaternion.Euler(0f, -90f, 0f);

                var visual = _partnerCharacter.AddComponent<CharacterVisualApplier>();
                var partnerAppearance = new CharacterAppearance
                {
                    Gender = GenderPreset.Male,
                    HairStyleId = "hair_short_01",
                    HairColorHex = "#2B1D0C",
                    EyeColorHex = "#3A75C4",
                    OutfitTopId = "casual_chic_01"
                };
                visual.ApplyAppearance(partnerAppearance);

                // Attach interaction targets and networking
                _partnerCharacter.AddComponent<InteractionDetector>();
                _partnerCharacter.AddComponent<NetworkCharacterState>();

                // Companion follower for offline development simulation
                _partnerCharacter.AddComponent<OfflinePartnerCompanion>();
            }
        }

        private void SetupCamera()
        {
            _mainCamera = GameObject.FindWithTag("MainCamera");
            if (_mainCamera == null)
            {
                _mainCamera = new GameObject("Main Camera");
                _mainCamera.tag = "MainCamera";
                _mainCamera.AddComponent<UnityEngine.Camera>();
                _mainCamera.AddComponent<AudioListener>();
            }

            var camCtrl = _mainCamera.GetComponent<CameraController>();
            if (camCtrl == null)
            {
                camCtrl = _mainCamera.AddComponent<CameraController>();
            }
            camCtrl.SetTarget(_localPlayer.transform);

            var photoMode = _mainCamera.GetComponent<PhotoModeController>();
            if (photoMode == null)
            {
                _mainCamera.AddComponent<PhotoModeController>();
            }
        }
    }

    /// <summary>
    /// Lightweight companion AI for Offline Development Mode.
    /// Walks gently beside the player when walking, allowing testing of
    /// hand-holding and romantic interactions without an active second device.
    /// </summary>
    public sealed class OfflinePartnerCompanion : MonoBehaviour
    {
        private Transform _playerTransform;
        private float _followDistance = 2.0f;
        private float _moveSpeed = 4.0f;

        private void Start()
        {
            var player = GameObject.Find("Player_Local");
            if (player != null)
            {
                _playerTransform = player.transform;
            }
        }

        private void Update()
        {
            if (_playerTransform == null) return;

            float dist = Vector3.Distance(transform.position, _playerTransform.position);
            if (dist > _followDistance && dist < 12f)
            {
                var targetPos = _playerTransform.position + (_playerTransform.rotation * new Vector3(1.4f, 0f, -0.5f));
                targetPos.y = transform.position.y;

                transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
                var lookDir = (_playerTransform.position - transform.position).normalized;
                lookDir.y = 0f;
                if (lookDir != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
                }
            }
        }
    }
}
