using System;
using System.Collections.Generic;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Home
{
    [Serializable]
    public sealed class PlacedFurnitureData
    {
        public string InstanceId;
        public string ItemId;
        public Vector3 Position;
        public Vector3 RotationEuler;
    }

    [Serializable]
    public sealed class HomeLayoutData
    {
        public List<PlacedFurnitureData> Items = new List<PlacedFurnitureData>();
    }

    /// <summary>
    /// Handles grid snapping, overlap collision validation, rotation, and persistence of home furniture.
    /// </summary>
    public sealed class HomePlacementService : MonoBehaviour
    {
        [SerializeField] private float _gridSize = 0.5f;
        [SerializeField] private LayerMask _obstacleLayers = ~0;

        private readonly Dictionary<string, GameObject> _placedInstances = new Dictionary<string, GameObject>();

        public Vector3 SnapToGrid(Vector3 worldPosition)
        {
            float x = Mathf.Round(worldPosition.x / _gridSize) * _gridSize;
            float z = Mathf.Round(worldPosition.z / _gridSize) * _gridSize;
            return new Vector3(x, worldPosition.y, z);
        }

        public bool ValidatePlacement(Vector3 snappedPosition, Vector3 extents, Quaternion rotation)
        {
            var colliders = Physics.OverlapBox(snappedPosition, extents * 0.49f, rotation, _obstacleLayers, QueryTriggerInteraction.Ignore);
            return colliders.Length == 0;
        }

        public PlacedFurnitureData PlaceItem(FurnitureItem item, Vector3 worldPosition, float rotationDegrees)
        {
            if (item == null || item.Prefab == null) return null;

            var snapped = SnapToGrid(worldPosition);
            var rotation = Quaternion.Euler(0f, rotationDegrees, 0f);

            var instanceId = Guid.NewGuid().ToString("N");
            var go = Instantiate(item.Prefab, snapped, rotation, transform);
            _placedInstances[instanceId] = go;

            var data = new PlacedFurnitureData
            {
                InstanceId = instanceId,
                ItemId = item.Id,
                Position = snapped,
                RotationEuler = rotation.eulerAngles
            };

            Lg.Info($"Placed furniture: {item.DisplayName} at {snapped}");
            return data;
        }

        public bool RemoveItem(string instanceId)
        {
            if (_placedInstances.TryGetValue(instanceId, out var go))
            {
                Destroy(go);
                _placedInstances.Remove(instanceId);
                Lg.Info($"Removed furniture instance: {instanceId}");
                return true;
            }
            return false;
        }

        public string SerializeLayout()
        {
            var layout = new HomeLayoutData();
            foreach (var kvp in _placedInstances)
            {
                if (kvp.Value == null) continue;
                layout.Items.Add(new PlacedFurnitureData
                {
                    InstanceId = kvp.Key,
                    ItemId = kvp.Value.name,
                    Position = kvp.Value.transform.position,
                    RotationEuler = kvp.Value.transform.rotation.eulerAngles
                });
            }
            return JsonUtility.ToJson(layout);
        }
    }
}
