using NUnit.Framework;
using UnityEngine;
using Loveverse.Characters;

namespace Loveverse.Tests
{
    public class CharacterCustomizationTests
    {
        [Test]
        public void CharacterAppearance_CloneCreatesIndependentCopy()
        {
            var original = new CharacterAppearance
            {
                Gender = GenderPreset.Female,
                HairStyleId = "hair_01",
                HairColorHex = "#123456"
            };

            var copy = original.Clone();
            copy.HairColorHex = "#654321";

            Assert.AreNotEqual(original.HairColorHex, copy.HairColorHex);
            Assert.AreEqual("#123456", original.HairColorHex);
            Assert.AreEqual("#654321", copy.HairColorHex);
        }

        [Test]
        public void CharacterAppearance_ColorHexParsingReturnsValidColor()
        {
            var appearance = new CharacterAppearance
            {
                SkinToneHex = "#FF0000" // Pure red
            };

            var color = appearance.GetSkinColor();
            Assert.AreEqual(1.0f, color.r, 0.01f);
            Assert.AreEqual(0.0f, color.g, 0.01f);
            Assert.AreEqual(0.0f, color.b, 0.01f);
        }
    }
}
