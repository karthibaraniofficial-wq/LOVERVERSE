using NUnit.Framework;
using UnityEngine;

namespace Loveverse.Tests
{
    public class CameraTests
    {
        [Test]
        public void PitchClamping_ConstrainsWithinBounds()
        {
            float minPitch = -30f;
            float maxPitch = 65f;

            float tooLow = -45f;
            float tooHigh = 80f;
            float valid = 15f;

            Assert.AreEqual(minPitch, Mathf.Clamp(tooLow, minPitch, maxPitch));
            Assert.AreEqual(maxPitch, Mathf.Clamp(tooHigh, minPitch, maxPitch));
            Assert.AreEqual(valid, Mathf.Clamp(valid, minPitch, maxPitch));
        }

        [Test]
        public void TargetPivotOffset_CalculatesCorrectElevation()
        {
            var characterPos = new Vector3(10f, 0f, 5f);
            var offset = new Vector3(0f, 1.4f, 0f);

            var pivot = characterPos + offset;

            Assert.AreEqual(10f, pivot.x);
            Assert.AreEqual(1.4f, pivot.y);
            Assert.AreEqual(5f, pivot.z);
        }
    }
}
