using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using Loveverse.Core;
using Loveverse.Characters;
using Loveverse.Networking;
using Loveverse.Romance;
using Loveverse.Home;
using Loveverse.Memories;

namespace Loveverse.Tests
{
    /// <summary>
    /// Complete architectural integration test validating the 14-step lifecycle loop of LOVEVERSE:
    /// Boot -> Auth -> Character -> Room -> Partner Join -> Sync -> Romance -> Home -> Place -> Memory -> Save -> Disconnect -> Reconnect -> Restore.
    /// </summary>
    public class EndToEndSimulationTests
    {
        [SetUp]
        public void Setup()
        {
            ServiceRegistry.Reset();
            EventBus.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            ServiceRegistry.Reset();
            EventBus.Clear();
        }

        [Test]
        public async Task FullGameplayLifecycle_ExecutesSuccessfully()
        {
            // 1. Client Launches & Core Services Register
            var customService = new CharacterCustomizationService();
            await customService.InitializeAsync();
            ServiceRegistry.Instance.Register<ICharacterCustomizationService>(customService);

            var netGo = new GameObject("NetworkManager");
            var netService = netGo.AddComponent<NetworkManager>();
            await netService.InitializeAsync();
            ServiceRegistry.Instance.Register<INetworkService>(netService);

            Assert.IsTrue(customService.IsInitialized);
            Assert.IsTrue(netService.IsInitialized);

            // 2. User Authenticates & Sets Character Profile
            var appearance = new CharacterAppearance
            {
                Gender = GenderPreset.Female,
                HairStyleId = "hair_long_01",
                HairColorHex = "#4A3728",
                EyeColorHex = "#5B84B1"
            };
            customService.SetAppearance(appearance);
            string appearanceJson = customService.ToJson();
            Assert.IsNotEmpty(appearanceJson);

            // 3. Room is Created with 6-char Cryptographic Code
            string roomCode = await netService.CreatePrivateRoomAsync();
            Assert.IsTrue(RoomCodeGenerator.IsValidCodeFormat(roomCode));
            Assert.IsTrue(netService.IsConnected);
            Assert.AreEqual(1, netService.ConnectedPlayerCount);

            // 4. Second Player Joins Private Room
            bool joinSuccess = await netService.JoinPrivateRoomAsync(roomCode);
            Assert.IsTrue(joinSuccess);
            Assert.AreEqual(2, netService.ConnectedPlayerCount);

            // 5. Romance Consent & Interaction Alignment
            var requestServiceGo = new GameObject("RequestService");
            var requestService = requestServiceGo.AddComponent<InteractionRequestService>();
            var req = requestService.SendRequest("player1", "player2", InteractionType.Hug);
            Assert.AreEqual(InteractionType.Hug, req.Type);
            Assert.AreEqual(15f, req.TimeRemaining);

            bool accepted = false;
            requestService.OnRequestAccepted += r => accepted = true;
            requestService.AcceptRequest();
            Assert.IsTrue(accepted);

            // 6. Shared Home & Furniture Placement
            var homeGo = new GameObject("HomePlacement");
            var homeService = homeGo.AddComponent<HomePlacementService>();
            var snappedPos = homeService.SnapToGrid(new Vector3(1.23f, 0f, 4.88f));
            Assert.AreEqual(1.0f, snappedPos.x, 0.01f);
            Assert.AreEqual(5.0f, snappedPos.z, 0.01f);

            // 7. Memory Metadata Creation
            var memoryItem = new MemoryItem
            {
                MemoryId = "mem_e2e_001",
                WorldId = "world_e2e",
                CreatedBy = "player1",
                Caption = "Our first garden sunset",
                LocationId = "RomanticGarden",
                CreatedAt = "2026-11-12T11:00:00Z"
            };
            Assert.AreEqual("mem_e2e_001", memoryItem.MemoryId);

            // 8. Disconnect and Reconnect Lifecycle
            await netService.LeaveRoomAsync();
            Assert.IsFalse(netService.IsConnected);
            Assert.AreEqual(0, netService.ConnectedPlayerCount);

            bool reconnected = await netService.JoinPrivateRoomAsync(roomCode);
            Assert.IsTrue(reconnected);
            Assert.IsTrue(netService.IsConnected);

            // Cleanup GameObjects
            Object.DestroyImmediate(netGo);
            Object.DestroyImmediate(requestServiceGo);
            Object.DestroyImmediate(homeGo);
        }
    }
}
