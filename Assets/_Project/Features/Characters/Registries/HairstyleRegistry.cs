using System.Collections.Generic;
using UnityEngine;

namespace Loveverse.Characters
{
    [CreateAssetMenu(fileName = "HairstyleRegistry", menuName = "Loveverse/Customization/Registries/Hairstyles")]
    public sealed class HairstyleRegistry : ScriptableObject
    {
        [SerializeField] private List<CustomizationItem> _items = new List<CustomizationItem>();
        public IReadOnlyList<CustomizationItem> Items => _items;

        public CustomizationItem FindById(string id)
        {
            return _items.Find(item => item.Id == id);
        }
    }
}
