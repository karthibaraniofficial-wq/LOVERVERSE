using NUnit.Framework;
using UnityEngine;
using Loveverse.Memories;

namespace Loveverse.Tests
{
    public class MemoryMetadataTests
    {
        [Test]
        public void MemoryItem_InitializesAndSerializesToJson()
        {
            var memory = new MemoryItem
            {
                MemoryId = "mem_12345",
                WorldId = "world_garden",
                CreatedBy = "user_alpha",
                Caption = "Sunset by the lily pond",
                LocationId = "LoveTown_Garden",
                LocalFilePath = "/path/to/mem_12345.png",
                CreatedAt = "2026-11-12T11:00:00Z"
            };

            string json = JsonUtility.ToJson(memory);
            Assert.IsNotNull(json);
            Assert.IsTrue(json.Contains("mem_12345"));
            Assert.IsTrue(json.Contains("Sunset by the lily pond"));

            var deserialized = JsonUtility.FromJson<MemoryItem>(json);
            Assert.AreEqual("mem_12345", deserialized.MemoryId);
            Assert.AreEqual("Sunset by the lily pond", deserialized.Caption);
            Assert.AreEqual("LoveTown_Garden", deserialized.LocationId);
        }
    }
}

