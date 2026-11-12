using System;
using System.Threading.Tasks;
using Loveverse.Core;

namespace Loveverse.Characters
{
    public interface ICharacterCustomizationService : IGameService
    {
        CharacterAppearance CurrentAppearance { get; }
        event Action<CharacterAppearance> OnAppearanceChanged;

        void SetAppearance(CharacterAppearance appearance);
        void SetHair(string hairStyleId, string hairColorHex);
        void SetOutfit(string topId, string bottomId, string shoesId);
        string ToJson();
        void LoadFromJson(string json);
    }

    public sealed class CharacterCustomizationService : ICharacterCustomizationService
    {
        public bool IsInitialized { get; private set; }
        public CharacterAppearance CurrentAppearance { get; private set; } = new CharacterAppearance();

        public event Action<CharacterAppearance> OnAppearanceChanged;

        public Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public void Shutdown()
        {
            IsInitialized = false;
        }

        public void SetAppearance(CharacterAppearance appearance)
        {
            if (appearance == null) return;
            CurrentAppearance = appearance.Clone();
            OnAppearanceChanged?.Invoke(CurrentAppearance);
        }

        public void SetHair(string hairStyleId, string hairColorHex)
        {
            CurrentAppearance.HairStyleId = hairStyleId;
            CurrentAppearance.HairColorHex = hairColorHex;
            OnAppearanceChanged?.Invoke(CurrentAppearance);
        }

        public void SetOutfit(string topId, string bottomId, string shoesId)
        {
            CurrentAppearance.OutfitTopId = topId;
            CurrentAppearance.OutfitBottomId = bottomId;
            CurrentAppearance.OutfitShoesId = shoesId;
            OnAppearanceChanged?.Invoke(CurrentAppearance);
        }

        public string ToJson()
        {
            return UnityEngine.JsonUtility.ToJson(CurrentAppearance);
        }

        public void LoadFromJson(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            var data = UnityEngine.JsonUtility.FromJson<CharacterAppearance>(json);
            if (data != null)
            {
                SetAppearance(data);
            }
        }
    }
}
