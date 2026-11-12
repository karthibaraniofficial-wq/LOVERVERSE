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
        [SerializeField] private string _initialScene = "MainMenu";
        [SerializeField] private bool _autoLoadInitialScene = true;

        private async void Awake()
        {
            if (_isBootstrapped)
            {
                Destroy(gameObject);
                return;
            }

            _isBootstrapped = true;
            DontDestroyOnLoad(gameObject);

            Lg.Info("========================================");
            Lg.Info("   💖 LOVEVERSE — Game Bootstrapper     ");
            Lg.Info("========================================");

            await InitializeCoreServicesAsync();
        }

        private async void Start()
        {
            if (_autoLoadInitialScene && !string.IsNullOrEmpty(_initialScene))
            {
                if (ServiceRegistry.Instance.TryResolve<ISceneService>(out var sceneService))
                {
                    Lg.Info($"Auto-loading initial scene: {_initialScene}");
                    await sceneService.LoadSceneAsync(_initialScene);
                }
            }
        }

        private async Task InitializeCoreServicesAsync()
        {
            // 1. Register PlatformService
            if (!ServiceRegistry.Instance.TryResolve<IPlatformService>(out _))
            {
                var platformService = GetComponentInChildren<PlatformService>();
                if (platformService == null)
                {
                    var platformGo = new GameObject("[Service] PlatformService");
                    platformGo.transform.SetParent(transform);
                    platformService = platformGo.AddComponent<PlatformService>();
                }
                await platformService.InitializeAsync();
                ServiceRegistry.Instance.Register<IPlatformService>(platformService);
            }

            // 2. Register SceneService
            if (!ServiceRegistry.Instance.TryResolve<ISceneService>(out _))
            {
                var sceneService = GetComponentInChildren<SceneService>();
                if (sceneService == null)
                {
                    var sceneServiceGo = new GameObject("[Service] SceneService");
                    sceneServiceGo.transform.SetParent(transform);
                    sceneService = sceneServiceGo.AddComponent<SceneService>();
                }
                await sceneService.InitializeAsync();
                ServiceRegistry.Instance.Register<ISceneService>(sceneService);
            }

            // 3. Register AuthService
            if (!ServiceRegistry.Instance.TryResolve<IAuthService>(out _))
            {
                var authService = GetComponentInChildren<AuthService>();
                if (authService == null)
                {
                    var authGo = new GameObject("[Service] AuthService");
                    authGo.transform.SetParent(transform);
                    authService = authGo.AddComponent<AuthService>();
                }
                await authService.InitializeAsync();
                ServiceRegistry.Instance.Register<IAuthService>(authService);
            }

            // 4. Register BackendService
            if (!ServiceRegistry.Instance.TryResolve<IBackendService>(out _))
            {
                var backendService = GetComponentInChildren<BackendService>();
                if (backendService == null)
                {
                    var backendGo = new GameObject("[Service] BackendService");
                    backendGo.transform.SetParent(transform);
                    backendService = backendGo.AddComponent<BackendService>();
                }
                await backendService.InitializeAsync();
                ServiceRegistry.Instance.Register<IBackendService>(backendService);
            }

            Lg.Info("Core Services (Platform, Scene, Auth, Backend) successfully registered.");
        }
    }
}
