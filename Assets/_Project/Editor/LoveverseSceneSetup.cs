using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Loveverse.Core;
using Loveverse.UI;
using Loveverse.World;
using Loveverse.Characters;
using Loveverse.Networking;
using Loveverse.Memories;

namespace Loveverse.Editor
{
    /// <summary>
    /// Automated scene setup and build settings orchestrator.
    /// Creates the complete 11-scene suite for LOVEVERSE and registers them in EditorBuildSettings.
    /// </summary>
    [InitializeOnLoad]
    public static class LoveverseSceneSetup
    {
        private const string ScenesFolder = "Assets/_Project/Scenes";

        private static readonly (string Name, string Path, System.Action<UnityEngine.SceneManagement.Scene> Setup)[] SceneDefinitions =
        {
            ("Bootstrap", "Assets/_Project/Scenes/Bootstrap.unity", SetupBootstrapScene),
            ("MainMenu", "Assets/_Project/Scenes/MainMenu.unity", SetupMainMenuScene),
            ("Authentication", "Assets/_Project/Scenes/Authentication.unity", SetupAuthenticationScene),
            ("CharacterCreator", "Assets/_Project/Scenes/CharacterCreator.unity", SetupCharacterCreatorScene),
            ("RoomLobby", "Assets/_Project/Scenes/RoomLobby.unity", SetupRoomLobbyScene),
            ("LoveTown", "Assets/_Project/Scenes/LoveTown.unity", s => SetupWorldScene(s, WorldRegionType.LoveTown, "Love Town")),
            ("OurHome", "Assets/_Project/Scenes/OurHome.unity", s => SetupWorldScene(s, WorldRegionType.OurHome, "Our Home")),
            ("SunsetBeach", "Assets/_Project/Scenes/SunsetBeach.unity", s => SetupWorldScene(s, WorldRegionType.SunsetBeach, "Sunset Beach")),
            ("DreamForest", "Assets/_Project/Scenes/DreamForest.unity", s => SetupWorldScene(s, WorldRegionType.DreamForest, "Dream Forest")),
            ("SkyMountain", "Assets/_Project/Scenes/SkyMountain.unity", s => SetupWorldScene(s, WorldRegionType.SkyMountain, "Sky Mountain")),
            ("StarlightCity", "Assets/_Project/Scenes/StarlightCity.unity", s => SetupWorldScene(s, WorldRegionType.StarlightCity, "Starlight City"))
        };

        static LoveverseSceneSetup()
        {
            EditorApplication.delayCall += CheckAndGenerateScenes;
        }

        private static void CheckAndGenerateScenes()
        {
            if (!File.Exists("Assets/_Project/Scenes/Bootstrap.unity"))
            {
                GenerateAllScenes();
            }
        }

        [MenuItem("LOVEVERSE/Generate & Configure All Scenes")]
        public static void GenerateAllScenes()
        {
            if (!Directory.Exists(ScenesFolder))
            {
                Directory.CreateDirectory(ScenesFolder);
                AssetDatabase.Refresh();
            }

            var buildScenes = new EditorBuildSettingsScene[SceneDefinitions.Length];

            for (int i = 0; i < SceneDefinitions.Length; i++)
            {
                var def = SceneDefinitions[i];
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

                def.Setup(scene);

                EditorSceneManager.SaveScene(scene, def.Path);
                buildScenes[i] = new EditorBuildSettingsScene(def.Path, true);
                Debug.Log($"[LOVEVERSE] Created & Saved Scene: {def.Name} -> {def.Path}");
            }

            // Register into EditorBuildSettings
            EditorBuildSettings.scenes = buildScenes;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[LOVEVERSE] Registered {buildScenes.Length} scenes into EditorBuildSettings.");

            // Open Bootstrap as the active editor scene
            EditorSceneManager.OpenScene("Assets/_Project/Scenes/Bootstrap.unity", OpenSceneMode.Single);
            Debug.Log("[LOVEVERSE] Active scene set to Bootstrap. Ready for Unity Play Mode!");
        }

        private static void CreateCameraAndLight(string cameraName = "Main Camera")
        {
            var camGo = new GameObject(cameraName);
            camGo.tag = "MainCamera";
            var cam = camGo.AddComponent<UnityEngine.Camera>();
            cam.clearFlags = CameraClearFlags.Skybox;
            camGo.AddComponent<AudioListener>();
            camGo.transform.position = new Vector3(0f, 1.6f, -3.5f);
            camGo.transform.rotation = Quaternion.Euler(15f, 0f, 0f);

            var lightGo = new GameObject("Directional Light");
            var l = lightGo.AddComponent<Light>();
            l.type = LightType.Directional;
            l.color = new Color(1f, 0.95f, 0.88f);
            l.intensity = 1.1f;
            lightGo.transform.rotation = Quaternion.Euler(45f, -30f, 0f);
        }

        private static void SetupBootstrapScene(UnityEngine.SceneManagement.Scene scene)
        {
            CreateCameraAndLight();

            var bootGo = new GameObject("[LOVEVERSE_BOOTSTRAP]");
            bootGo.AddComponent<GameBootstrapper>();
            bootGo.AddComponent<PlatformService>();
            bootGo.AddComponent<SceneService>();
            bootGo.AddComponent<AuthService>();
            bootGo.AddComponent<BackendService>();
            bootGo.AddComponent<NetworkManager>();
            bootGo.AddComponent<CharacterCustomizationComponent>();
            bootGo.AddComponent<MemoryService>();
        }

        private static void SetupMainMenuScene(UnityEngine.SceneManagement.Scene scene)
        {
            CreateCameraAndLight();

            var menuGo = new GameObject("[UI_MainMenu]");
            menuGo.AddComponent<MainMenuController>();
        }

        private static void SetupAuthenticationScene(UnityEngine.SceneManagement.Scene scene)
        {
            CreateCameraAndLight();

            var authGo = new GameObject("[UI_Authentication]");
            authGo.AddComponent<AuthenticationUI>();
        }

        private static void SetupCharacterCreatorScene(UnityEngine.SceneManagement.Scene scene)
        {
            CreateCameraAndLight();

            // Preview Avatar Pedestal
            var pedestal = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pedestal.name = "Preview_Pedestal";
            pedestal.transform.position = new Vector3(0.8f, 0.1f, 0f);
            pedestal.transform.localScale = new Vector3(2f, 0.2f, 2f);
            pedestal.GetComponent<Renderer>().material.color = new Color(0.9f, 0.9f, 0.95f);

            var avatar = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            avatar.name = "Preview_Avatar";
            avatar.transform.position = new Vector3(0.8f, 1.2f, 0f);
            avatar.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            avatar.AddComponent<CharacterVisualApplier>();

            var creatorGo = new GameObject("[UI_CharacterCreator]");
            creatorGo.AddComponent<CharacterCreatorUI>();
        }

        private static void SetupRoomLobbyScene(UnityEngine.SceneManagement.Scene scene)
        {
            CreateCameraAndLight();

            var lobbyGo = new GameObject("[UI_RoomLobby]");
            lobbyGo.AddComponent<RoomLobbyUI>();
        }

        private static void SetupWorldScene(UnityEngine.SceneManagement.Scene scene, WorldRegionType regionType, string regionName)
        {
            var worldGo = new GameObject($"[WorldRegion_{regionType}]");
            var builder = worldGo.AddComponent<WorldRegionBuilder>();

            // Set private fields via reflection or properties if needed, or Awake sets it up
            var typeField = typeof(WorldRegionBuilder).GetField("_regionType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (typeField != null) typeField.SetValue(builder, regionType);

            var nameField = typeof(WorldRegionBuilder).GetField("_regionDisplayName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (nameField != null) nameField.SetValue(builder, regionName);

            var hudGo = new GameObject("WorldNavigationHUD");
            var hud = hudGo.AddComponent<WorldNavigationHUD>();
            var regionField = typeof(WorldNavigationHUD).GetField("_currentRegionName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (regionField != null) regionField.SetValue(hud, regionName);
        }
    }
}
