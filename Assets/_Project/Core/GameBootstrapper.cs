using System.Threading.Tasks;
using UnityEngine;

namespace Loveverse.Core
{
    /// <summary>
    /// Master entry point that initializes the foundational services and orchestrates game startup.
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
            // Register SceneService
            var sceneServiceGo = new GameObject("[Service] SceneService");
            sceneServiceGo.transform.SetParent(transform);
            var sceneService = sceneServiceGo.AddComponent<SceneService>();
            await sceneService.InitializeAsync();
            ServiceRegistry.Instance.Register<ISceneService>(sceneService);

            Lg.Info("Foundational Core Services successfully registered.");
        }
    }
}
