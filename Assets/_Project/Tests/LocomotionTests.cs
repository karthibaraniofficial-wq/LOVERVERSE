using NUnit.Framework;
using UnityEngine;
using Loveverse.Movement;

namespace Loveverse.Tests
{
    public class LocomotionTests
    {
        [Test]
        public void JumpVelocityFormula_ComputesCorrectInitialVelocity()
        {
            // Formula: v = sqrt(2 * h * -g)
            float jumpHeight = 1.4f;
            float gravity = -18.0f;

            float expectedVelocity = Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(gravity));
            Assert.AreEqual(7.099f, expectedVelocity, 0.01f);
        }

        [Test]
        public void InputVector_NormalizationLimitsDiagonalSpeed()
        {
            var rawInput = new Vector2(1f, 1f);
            var normalized = rawInput.normalized;

            Assert.AreEqual(1.0f, normalized.magnitude, 0.001f);
            Assert.AreEqual(0.7071f, normalized.x, 0.001f);
            Assert.AreEqual(0.7071f, normalized.y, 0.001f);
        }
    }
}
