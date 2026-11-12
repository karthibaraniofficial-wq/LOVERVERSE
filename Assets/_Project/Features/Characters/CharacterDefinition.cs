using UnityEngine;

namespace Loveverse.Characters
{
    [CreateAssetMenu(fileName = "CharacterDefinition", menuName = "Loveverse/Characters/CharacterDefinition")]
    public sealed class CharacterDefinition : ScriptableObject
    {
        [SerializeField] private string _archetypeId = "anime_humanoid_base";
        [SerializeField] private GenderPreset _gender = GenderPreset.Female;
        [SerializeField] private GameObject _baseRigPrefab;
        [SerializeField] private RuntimeAnimatorController _animatorController;
        [SerializeField] private Avatar _humanoidAvatar;

        public string ArchetypeId => _archetypeId;
        public GenderPreset Gender => _gender;
        public GameObject BaseRigPrefab => _baseRigPrefab;
        public RuntimeAnimatorController AnimatorController => _animatorController;
        public Avatar HumanoidAvatar => _humanoidAvatar;
    }
}
