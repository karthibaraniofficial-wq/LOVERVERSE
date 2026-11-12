using System.Collections.Generic;
using UnityEngine;

namespace Loveverse.Home
{
    [CreateAssetMenu(fileName = "FurnitureRegistry", menuName = "Loveverse/Home/FurnitureRegistry")]
    public sealed class FurnitureRegistry : ScriptableObject
    {
        [SerializeField] private List<FurnitureItem> _items = new List<FurnitureItem>();
        public IReadOnlyList<FurnitureItem> Items => _items;

        public FurnitureItem FindById(string id)
        {
            return _items.Find(item => item.Id == id);
        }
    }
}
