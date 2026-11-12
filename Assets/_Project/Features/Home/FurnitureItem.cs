using UnityEngine;

namespace Loveverse.Home
{
    public enum FurnitureCategory
    {
        Seating,
        Tables,
        Beds,
        Decorations,
        Lighting,
        Outdoor
    }

    [CreateAssetMenu(fileName = "FurnitureItem", menuName = "Loveverse/Home/FurnitureItem")]
    public sealed class FurnitureItem : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private FurnitureCategory _category;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(1, 1);
        [SerializeField] private Sprite _icon;

        public string Id => _id;
        public string DisplayName => _displayName;
        public FurnitureCategory Category => _category;
        public GameObject Prefab => _prefab;
        public Vector2Int GridSize => _gridSize;
        public Sprite Icon => _icon;
    }
}
