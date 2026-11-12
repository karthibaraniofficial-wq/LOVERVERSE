using NUnit.Framework;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Tests
{
    public class PlatformTests
    {
        [Test]
        public void PlatformDetector_IdentifiesMobileAndDesktopCorrectly()
        {
            Assert.IsTrue(PlatformDetector.IsMobile(PlatformType.Android));
            Assert.IsTrue(PlatformDetector.IsMobile(PlatformType.iOS));
            Assert.IsFalse(PlatformDetector.IsMobile(PlatformType.Windows));
            Assert.IsFalse(PlatformDetector.IsMobile(PlatformType.Linux));

            Assert.IsTrue(PlatformDetector.IsDesktop(PlatformType.Windows));
            Assert.IsTrue(PlatformDetector.IsDesktop(PlatformType.Linux));
            Assert.IsFalse(PlatformDetector.IsDesktop(PlatformType.Android));
            Assert.IsFalse(PlatformDetector.IsDesktop(PlatformType.iOS));
        }

        [Test]
        public void PlatformStorageAdapter_NormalizesPathsSafely()
        {
            string memoriesPath = PlatformStorageAdapter.GetMemoriesDirectory();
            Assert.IsNotNull(memoriesPath);
            Assert.IsTrue(memoriesPath.EndsWith("Memories"));

            string settingsPath = PlatformStorageAdapter.GetSettingsFilePath();
            Assert.IsNotNull(settingsPath);
            Assert.IsTrue(settingsPath.EndsWith("settings.json"));
        }

        [Test]
        public void GraphicsQualityConfig_ReturnsValidScalesForEachTier()
        {
            var config = ScriptableObject.CreateInstance<GraphicsQualityConfig>();

            var low = config.GetPreset(QualityTier.Low);
            Assert.AreEqual(0.75f, low.RenderScale, 0.01f);
            Assert.IsFalse(low.SoftShadows);

            var medium = config.GetPreset(QualityTier.Medium);
            Assert.AreEqual(0.9f, medium.RenderScale, 0.01f);

            var high = config.GetPreset(QualityTier.High);
            Assert.AreEqual(1.0f, high.RenderScale, 0.01f);

            var ultra = config.GetPreset(QualityTier.Ultra);
            Assert.AreEqual(100, ultra.ShadowDistance);

            Object.DestroyImmediate(config);
        }

        [Test]
        public void PlatformConfiguration_RetrievesEntriesForAllFourPlatforms()
        {
            var config = ScriptableObject.CreateInstance<PlatformConfiguration>();

            var android = config.GetSettingsForPlatform(PlatformType.Android);
            Assert.AreEqual(PlatformType.Android, android.Platform);
            Assert.IsTrue(android.EnableVirtualJoystick);

            var ios = config.GetSettingsForPlatform(PlatformType.iOS);
            Assert.AreEqual(PlatformType.iOS, ios.Platform);
            Assert.IsTrue(ios.EnableVirtualJoystick);

            var win = config.GetSettingsForPlatform(PlatformType.Windows);
            Assert.AreEqual(PlatformType.Windows, win.Platform);
            Assert.IsFalse(win.EnableVirtualJoystick);

            var linux = config.GetSettingsForPlatform(PlatformType.Linux);
            Assert.AreEqual(PlatformType.Linux, linux.Platform);
            Assert.IsFalse(linux.EnableVirtualJoystick);

            Object.DestroyImmediate(config);
        }
    }
}

