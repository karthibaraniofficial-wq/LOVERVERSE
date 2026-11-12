using System.Threading.Tasks;
using UnityEngine;

namespace Loveverse.Core
{
    /// <summary>
    /// Master entry point that initializes the foundational services and orchestrates game startup.
    /// Supports Android, iOS, Windows, and Linux platform bootstrap.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public sealed class GameBootstrapper : MonoBehaviour
    {
        private static bool _isBootstrapped;

        [Header("Startup Configuration")]
        [SerializeField] private string _initialGameplayScene = "LoveTown";
        [SerializeField] private bool _autoLoadInitialScene = false;

        private async void Awake()
        {
            if (_isBootstrapped)
            {
                Destroy(gameObject);
                return;
            }

            _isBootstrapped = true;
            DontDestroyOnLoad(gameObject);

            Lg.Info("Starting LOVEVERSE Bootstrapper...");
            await InitializeCoreServicesAsync();

            if (_autoLoadInitialScene)
            {
                var sceneService = ServiceRegistry.Instance.Resolve<ISceneService>();
                await sceneService.LoadSceneAsync(_initialGameplayScene);
            }
        }

        private async Task InitializeCoreServicesAsync()
        {
            // 1. Register PlatformService
            var platformGo = new GameObject("[Service] PlatformService");
            platformGo.transform.SetParent(transform);
            var platformService = platformGo.AddComponent<PlatformService>();
            await platformService.InitializeAsync();
            ServiceRegistry.Instance.Register<IPlatformService>(platformService);

            // 2. Register SceneService
            var sceneServiceGo = new GameObject("[Service] SceneService");
            sceneServiceGo.transform.SetParent(transform);
            var sceneService = sceneServiceGo.AddComponent<SceneService>();
            await sceneService.InitializeAsync();
            ServiceRegistry.Instance.Register<ISceneService>(sceneService);

            Lg.Info("Foundational Core Services (Platform + Scene) successfully registered.");
        }
    }
}
