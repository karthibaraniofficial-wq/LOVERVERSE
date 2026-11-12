using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Memories
{
    public interface IMemoryService : IGameService
    {
        IReadOnlyList<MemoryItem> LocalMemories { get; }
        event Action<MemoryItem> OnMemorySaved;

        Task<MemoryItem> SaveMemoryAsync(Texture2D texture, string worldId, string caption, string locationId);
    }

    /// <summary>
    /// Captures photo textures, saves PNGs to local storage, and prepares metadata payloads.
    /// </summary>
    public sealed class MemoryService : MonoBehaviour, IMemoryService
    {
        public bool IsInitialized { get; private set; }
        private readonly List<MemoryItem> _memories = new List<MemoryItem>();
        public IReadOnlyList<MemoryItem> LocalMemories => _memories;

        public event Action<MemoryItem> OnMemorySaved;

        private string StorageDirectory => Path.Combine(Application.persistentDataPath, "Memories");

        private async void Awake()
        {
            if (ServiceRegistry.Instance.TryResolve<IMemoryService>(out _))
            {
                return;
            }

            await InitializeAsync();
            ServiceRegistry.Instance.Register<IMemoryService>(this);
        }

        public Task InitializeAsync()
        {
            if (!Directory.Exists(StorageDirectory))
            {
                Directory.CreateDirectory(StorageDirectory);
            }

            IsInitialized = true;
            Lg.Info($"MemoryService initialized. Storage at: {StorageDirectory}");
            return Task.CompletedTask;
        }

        public void Shutdown()
        {
            IsInitialized = false;
        }

        public async Task<MemoryItem> SaveMemoryAsync(Texture2D texture, string worldId, string caption, string locationId)
        {
            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            var memoryId = $"mem_{DateTime.UtcNow.Ticks}";
            var fileName = $"{memoryId}.png";
            var filePath = Path.Combine(StorageDirectory, fileName);

            // Encode PNG
            byte[] pngBytes = texture.EncodeToPNG();
            await File.WriteAllBytesAsync(filePath, pngBytes);

            var item = new MemoryItem
            {
                MemoryId = memoryId,
                WorldId = worldId,
                CreatedBy = "local_player",
                Caption = caption,
                LocationId = locationId,
                LocalFilePath = filePath,
                CreatedAt = DateTime.UtcNow.ToString("o")
            };

            _memories.Add(item);
            Lg.Info($"Memory saved to disk: {filePath}");
            OnMemorySaved?.Invoke(item);

            return item;
        }
    }
}
