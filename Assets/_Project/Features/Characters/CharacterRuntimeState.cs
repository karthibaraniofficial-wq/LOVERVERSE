using UnityEngine;

namespace Loveverse.Characters
{
    /// <summary>
    /// Tracks the live runtime customization state and equipped items of a character instance.
    /// </summary>
    public sealed class CharacterRuntimeState : MonoBehaviour
    {
        [SerializeField] private CharacterAppearance _activeAppearance = new CharacterAppearance();
        public CharacterAppearance ActiveAppearance => _activeAppearance;

        public void SetAppearance(CharacterAppearance appearance)
        {
            if (appearance == null) return;
            _activeAppearance = appearance.Clone();
        }
    }
}
