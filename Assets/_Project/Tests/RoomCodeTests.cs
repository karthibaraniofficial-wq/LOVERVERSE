using NUnit.Framework;
using Loveverse.Networking;

namespace Loveverse.Tests
{
    public class RoomCodeTests
    {
        [Test]
        public void Generate_ReturnsSixCharacterCode()
        {
            string code = RoomCodeGenerator.Generate();
            Assert.IsNotNull(code);
            Assert.AreEqual(6, code.Length);
        }

        [Test]
        public void Generate_DoesNotContainAmbiguousCharacters()
        {
            // Run multiple iterations to verify random character set
            for (int i = 0; i < 50; i++)
            {
                string code = RoomCodeGenerator.Generate();
                Assert.IsFalse(code.Contains("0"), "Code should not contain '0'");
                Assert.IsFalse(code.Contains("O"), "Code should not contain 'O'");
                Assert.IsFalse(code.Contains("1"), "Code should not contain '1'");
                Assert.IsFalse(code.Contains("I"), "Code should not contain 'I'");
                Assert.IsFalse(code.Contains("L"), "Code should not contain 'L'");
            }
        }

        [Test]
        public void IsValidCodeFormat_ValidatesCorrectly()
        {
            string validCode = RoomCodeGenerator.Generate();
            Assert.IsTrue(RoomCodeGenerator.IsValidCodeFormat(validCode));
            Assert.IsTrue(RoomCodeGenerator.IsValidCodeFormat(validCode.ToLowerInvariant()));

            Assert.IsFalse(RoomCodeGenerator.IsValidCodeFormat(""));
            Assert.IsFalse(RoomCodeGenerator.IsValidCodeFormat("SHORT"));
            Assert.IsFalse(RoomCodeGenerator.IsValidCodeFormat("TOOLONG123"));
            Assert.IsFalse(RoomCodeGenerator.IsValidCodeFormat("12345O")); // Contains excluded 'O'
            Assert.IsFalse(RoomCodeGenerator.IsValidCodeFormat("12345!")); // Contains symbol
        }
    }
}
