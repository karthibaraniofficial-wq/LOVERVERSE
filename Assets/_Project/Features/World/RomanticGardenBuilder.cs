using UnityEngine;
using Loveverse.Core;

namespace Loveverse.World
{
    /// <summary>
    /// Procedural layout constructor for the Romantic Garden hub.
    /// Spawns central gazebo, walking path boundaries, benches, streetlamps, and flowering trees.
    /// </summary>
    public sealed class RomanticGardenBuilder : MonoBehaviour
    {
        [Header("Prefabs / Placeholders")]
        [SerializeField] private GameObject _gazeboPrefab;
        [SerializeField] private GameObject _benchPrefab;
        [SerializeField] private GameObject _streetLampPrefab;
        [SerializeField] private GameObject _willowTreePrefab;
        [SerializeField] private GameObject _flowerBedPrefab;

        [Header("Garden Dimensions")]
        [SerializeField] private float _gardenRadius = 25f;
        [SerializeField] private int _benchCount = 4;
        [SerializeField] private int _treeCount = 8;

        public void BuildGardenLayout()
        {
            Lg.Info("Constructing Romantic Garden layout...");

            // 1. Central Gazebo
            if (_gazeboPrefab != null)
            {
                var gazebo = Instantiate(_gazeboPrefab, Vector3.zero, Quaternion.identity, transform);
                gazebo.name = "Central_Gazebo";
            }

            // 2. Circular Benches
            if (_benchPrefab != null)
            {
                for (int i = 0; i < _benchCount; i++)
                {
                    float angle = i * (360f / _benchCount) * Mathf.Deg2Rad;
                    var pos = new Vector3(Mathf.Cos(angle) * (_gardenRadius * 0.45f), 0f, Mathf.Sin(angle) * (_gardenRadius * 0.45f));
                    var rot = Quaternion.LookRotation(-pos.normalized, Vector3.up);
                    var bench = Instantiate(_benchPrefab, pos, rot, transform);
                    bench.name = $"Couple_Bench_{i + 1}";
                }
            }

            // 3. Perimeter Trees and Lamps
            if (_willowTreePrefab != null)
            {
                for (int i = 0; i < _treeCount; i++)
                {
                    float angle = (i * (360f / _treeCount) + 22.5f) * Mathf.Deg2Rad;
                    var pos = new Vector3(Mathf.Cos(angle) * _gardenRadius, 0f, Mathf.Sin(angle) * _gardenRadius);
                    var tree = Instantiate(_willowTreePrefab, pos, Quaternion.Euler(0f, Random.Range(0, 360), 0f), transform);
                    tree.name = $"Willow_Tree_{i + 1}";
                }
            }

            Lg.Info("Romantic Garden construction complete.");
        }
    }
}
