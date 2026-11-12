using NUnit.Framework;
using UnityEngine;
using Loveverse.Romance;

namespace Loveverse.Tests
{
    public class RomanceValidationTests
    {
        [Test]
        public void InteractionRequest_InitializesWithCorrectParameters()
        {
            var request = new InteractionRequest("p1", "p2", InteractionType.Hug, 15f);

            Assert.AreEqual("p1", request.InitiatorId);
            Assert.AreEqual("p2", request.TargetId);
            Assert.AreEqual(InteractionType.Hug, request.Type);
            Assert.AreEqual(15f, request.TimeRemaining);
            Assert.IsNotNull(request.RequestId);
        }

        [Test]
        public void AngleDetection_CorrectlyEvaluatesFacing()
        {
            var forward = Vector3.forward;
            var directlyAhead = (Vector3.forward * 2f).normalized;
            var directlyBehind = (-Vector3.forward * 2f).normalized;

            float angleAhead = Vector3.Angle(forward, directlyAhead);
            float angleBehind = Vector3.Angle(forward, directlyBehind);

            Assert.AreEqual(0f, angleAhead, 0.01f);
            Assert.AreEqual(180f, angleBehind, 0.01f);
            Assert.IsTrue(angleAhead <= 85f, "Directly ahead target should be in range");
            Assert.IsFalse(angleBehind <= 85f, "Behind target should be rejected");
        }

        [Test]
        public void DistanceValidation_DetectsOutOfBounds()
        {
            var posA = new Vector3(0f, 0f, 0f);
            var posClose = new Vector3(0f, 0f, 2.0f);
            var posFar = new Vector3(0f, 0f, 5.0f);

            float range = 2.5f;

            Assert.IsTrue(Vector3.Distance(posA, posClose) <= range);
            Assert.IsFalse(Vector3.Distance(posA, posFar) <= range);
        }
    }
}
