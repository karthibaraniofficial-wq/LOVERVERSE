using System.Collections.Generic;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Home
{
    /// <summary>
    /// Master manager for shared home decoration, item manipulation, and layout saving.
    /// </summary>
    public sealed class HomeManager : MonoBehaviour
    {
        [SerializeField] private HomePlacementService _placementService;
        [SerializeField] private FurnitureRegistry _registry;

        public bool TryPlaceFurniture(string itemId, Vector3 worldPosition, float rotationDegrees)
        {
            if (_registry == null) return false;

            var item = _registry.FindById(itemId);
            if (item == null)
            {
                Lg.Warning($"Furniture item '{itemId}' not found in registry.");
                return false;
            }

            var placed = _placementService.PlaceItem(item, worldPosition, rotationDegrees);
            return placed != null;
        }

        public bool TryRemoveFurniture(string instanceId)
        {
            return _placementService.RemoveItem(instanceId);
        }

        public string GetSavedLayoutJson()
        {
            return _placementService.SerializeLayout();
        }
    }
}
