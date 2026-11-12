using NUnit.Framework;
using UnityEngine;
using Loveverse.Home;

namespace Loveverse.Tests
{
    public class HomePlacementTests
    {
        [Test]
        public void SnapToGrid_SnapsCoordinatesCorrectly()
        {
            var go = new GameObject("TestPlacementService");
            var service = go.AddComponent<HomePlacementService>();

            var pos1 = new Vector3(0.48f, 0f, 0.99f);
            var snapped1 = service.SnapToGrid(pos1);
            Assert.AreEqual(0.5f, snapped1.x, 0.001f);
            Assert.AreEqual(1.0f, snapped1.z, 0.001f);

            var pos2 = new Vector3(0.12f, 0f, -0.68f);
            var snapped2 = service.SnapToGrid(pos2);
            Assert.AreEqual(0.0f, snapped2.x, 0.001f);
            Assert.AreEqual(-0.5f, snapped2.z, 0.001f);

            Object.DestroyImmediate(go);
        }

        [Test]
        public void LayoutSerialization_GeneratesValidJson()
        {
            var layout = new HomeLayoutData();
            layout.Items.Add(new PlacedFurnitureData
            {
                InstanceId = "inst_1",
                ItemId = "sofa_01",
                Position = new Vector3(1f, 0f, 2f),
                RotationEuler = new Vector3(0f, 90f, 0f)
            });

            string json = JsonUtility.ToJson(layout);
            Assert.IsNotNull(json);
            Assert.IsTrue(json.Contains("inst_1"));
            Assert.IsTrue(json.Contains("sofa_01"));

            var deserialized = JsonUtility.FromJson<HomeLayoutData>(json);
            Assert.AreEqual(1, deserialized.Items.Count);
            Assert.AreEqual("inst_1", deserialized.Items[0].InstanceId);
            Assert.AreEqual(1f, deserialized.Items[0].Position.x);
        }
    }
}
