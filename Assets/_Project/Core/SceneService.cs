using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Loveverse.Core
{
    public interface ISceneService : IGameService
    {
        string CurrentSceneName { get; }
        Task LoadSceneAsync(string sceneName, IProgress<float> progress = null);
    }

    public sealed class SceneService : MonoBehaviour, ISceneService
    {
        public bool IsInitialized { get; private set; }
        public string CurrentSceneName => SceneManager.GetActiveScene().name;

        public Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public void Shutdown()
        {
            IsInitialized = false;
        }

        public async Task LoadSceneAsync(string sceneName, IProgress<float> progress = null)
        {
            Lg.Info($"Starting asynchronous scene load: {sceneName}");
            var op = SceneManager.LoadSceneAsync(sceneName);
            if (op == null)
            {
                Lg.Error($"Scene '{sceneName}' could not be loaded. Ensure it is added to Build Settings.");
                return;
            }

            while (!op.isDone)
            {
                progress?.Report(op.progress);
                await Task.Yield();
            }

            progress?.Report(1.0f);
            EventBus.Publish(new SceneLoadedEvent(sceneName));
            Lg.Info($"Scene '{sceneName}' loaded successfully.");
        }
    }
}
