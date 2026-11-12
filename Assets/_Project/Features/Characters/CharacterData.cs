using System;
using UnityEngine;

namespace Loveverse.Characters
{
    public enum GenderPreset
    {
        Female = 0,
        Male = 1,
        Neutral = 2
    }

    [Serializable]
    public sealed class CharacterAppearance
    {
        public GenderPreset Gender = GenderPreset.Female;
        public string SkinToneHex = "#FFE0BD";
        public string HairStyleId = "hair_long_twintail_01";
        public string HairColorHex = "#4A3728";
        public string EyeColorHex = "#5B84B1";
        public string OutfitTopId = "top_school_blazer_01";
        public string OutfitBottomId = "bottom_pleated_skirt_01";
        public string OutfitShoesId = "shoes_loafers_01";
        public string AccessoryId = "acc_ribbon_01";

        public Color GetSkinColor() => ColorUtility.TryParseHtmlString(SkinToneHex, out var c) ? c : Color.white;
        public Color GetHairColor() => ColorUtility.TryParseHtmlString(HairColorHex, out var c) ? c : Color.black;
        public Color GetEyeColor() => ColorUtility.TryParseHtmlString(EyeColorHex, out var c) ? c : Color.blue;

        public CharacterAppearance Clone()
        {
            return new CharacterAppearance
            {
                Gender = this.Gender,
                SkinToneHex = this.SkinToneHex,
                HairStyleId = this.HairStyleId,
                HairColorHex = this.HairColorHex,
                EyeColorHex = this.EyeColorHex,
                OutfitTopId = this.OutfitTopId,
                OutfitBottomId = this.OutfitBottomId,
                OutfitShoesId = this.OutfitShoesId,
                AccessoryId = this.AccessoryId
            };
        }
    }
}
