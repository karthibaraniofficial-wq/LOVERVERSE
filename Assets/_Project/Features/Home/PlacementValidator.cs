using UnityEngine;

namespace Loveverse.Home
{
    public static class PlacementGrid
    {
        public const float GridCellSize = 0.5f;

        public static Vector3 Snap(Vector3 position)
        {
            float x = Mathf.Round(position.x / GridCellSize) * GridCellSize;
            float z = Mathf.Round(position.z / GridCellSize) * GridCellSize;
            return new Vector3(x, position.y, z);
        }
    }

    public static class PlacementValidator
    {
        public static bool IsClear(Vector3 center, Vector3 size, Quaternion rotation, LayerMask obstacleLayer)
        {
            var halfExtents = size * 0.49f;
            var hits = Physics.OverlapBox(center, halfExtents, rotation, obstacleLayer, QueryTriggerInteraction.Ignore);
            return hits.Length == 0;
        }
    }
}
