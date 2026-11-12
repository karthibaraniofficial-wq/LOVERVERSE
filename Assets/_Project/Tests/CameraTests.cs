using NUnit.Framework;
using UnityEngine;
using Loveverse.Camera;
using Loveverse.Movement;

namespace Loveverse.Tests
{
    public class CameraTests
    {
        private class FakeInputService : IInputService
        {
            public bool IsInitialized => true;
            public Vector2 MoveInput { get; set; }
            public Vector2 LookInput { get; set; }
            public bool IsSprintPressed { get; set; }
            public bool IsJumpTriggered { get; set; }
            public bool IsInteractTriggered { get; set; }
            public bool IsPhotoModeTriggered { get; set; }

            public System.Threading.Tasks.Task InitializeAsync() => System.Threading.Tasks.Task.CompletedTask;
            public void Shutdown() {}
            public void SetVirtualMoveInput(Vector2 move) => MoveInput = move;
            public void SetVirtualLookInput(Vector2 look) => LookInput = look;
            public void TriggerVirtualJump() => IsJumpTriggered = true;
            public void TriggerVirtualInteract() => IsInteractTriggered = true;
            public void ToggleVirtualSprint() => IsSprintPressed = !IsSprintPressed;
        }

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

        [Test]
        public void CameraController_CanBeInitializedAndSetMode()
        {
            var go = new GameObject("TestCamera");
            var cam = go.AddComponent<CameraController>();
            var fakeInput = new FakeInputService();

            cam.Initialize(fakeInput);
            cam.SetCameraMode(CameraMode.PhotoMode);

            Assert.AreEqual(CameraMode.PhotoMode, cam.CurrentMode);
            Object.DestroyImmediate(go);
        }

        [Test]
        public void PhotoModeController_ToggleTogglesActiveState()
        {
            var go = new GameObject("TestPhotoMode");
            var photoMode = go.AddComponent<PhotoModeController>();
            var fakeInput = new FakeInputService();

            photoMode.Initialize(fakeInput);
            Assert.IsFalse(photoMode.IsPhotoModeActive);

            photoMode.TogglePhotoMode();
            Assert.IsTrue(photoMode.IsPhotoModeActive);

            photoMode.TogglePhotoMode();
            Assert.IsFalse(photoMode.IsPhotoModeActive);

            Object.DestroyImmediate(go);
        }
    }
}
