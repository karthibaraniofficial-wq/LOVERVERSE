using UnityEngine;

namespace Loveverse.Characters
{
    public enum CustomizationCategory
    {
        Hair,
        Face,
        Top,
        Bottom,
        Shoes,
        Accessory
    }

    [CreateAssetMenu(fileName = "CustomizationItem", menuName = "Loveverse/Customization/Item")]
    public sealed class CustomizationItem : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private CustomizationCategory _category;
        [SerializeField] private Sprite _icon;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _isDefaultUnlocked = true;

        public string Id => _id;
        public string DisplayName => _displayName;
        public CustomizationCategory Category => _category;
        public Sprite Icon => _icon;
        public GameObject Prefab => _prefab;
        public bool IsDefaultUnlocked => _isDefaultUnlocked;
    }
}
