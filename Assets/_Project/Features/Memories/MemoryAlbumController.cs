using System.Collections.Generic;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Memories
{
    /// <summary>
    /// Displays saved memory cards, captions, and details in the shared in-game photo album UI.
    /// </summary>
    public sealed class MemoryAlbumController : MonoBehaviour
    {
        [SerializeField] private MemoryService _memoryService;
        private readonly List<MemoryItem> _albumItems = new List<MemoryItem>();

        public IReadOnlyList<MemoryItem> AlbumItems => _albumItems;

        private void Start()
        {
            if (_memoryService != null)
            {
                _memoryService.OnMemorySaved += HandleMemorySaved;
            }
        }

        private void OnDestroy()
        {
            if (_memoryService != null)
            {
                _memoryService.OnMemorySaved -= HandleMemorySaved;
            }
        }

        private void HandleMemorySaved(MemoryItem memory)
        {
            _albumItems.Add(memory);
            Lg.Info($"New photo memory added to shared album: {memory.Caption}");
        }
    }
}
