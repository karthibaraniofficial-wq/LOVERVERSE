using System.Threading.Tasks;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Movement
{
    /// <summary>
    /// Unified input reader handling both Windows desktop (Keyboard/Mouse) and Android touch inputs.
    /// </summary>
    public sealed class InputReader : MonoBehaviour, IInputService
    {
        public bool IsInitialized { get; private set; }

        private Vector2 _virtualMove;
        private Vector2 _virtualLook;
        private bool _virtualJumpTriggered;
        private bool _virtualInteractTriggered;
        private bool _virtualSprintToggled;

        public Vector2 MoveInput
        {
            get
            {
                if (_virtualMove.sqrMagnitude > 0.01f)
                {
                    return _virtualMove;
                }

                float h = 0f;
                float v = 0f;

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) v += 1f;
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) v -= 1f;
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f;
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h -= 1f;

                var raw = new Vector2(h, v);
                return raw.sqrMagnitude > 1f ? raw.normalized : raw;
            }
        }

        public Vector2 LookInput
        {
            get
            {
                if (_virtualLook.sqrMagnitude > 0.01f)
                {
                    var look = _virtualLook;
                    _virtualLook = Vector2.zero; // Consume delta
                    return look;
                }

                return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
        }

        public bool IsSprintPressed => Input.GetKey(KeyCode.LeftShift) || _virtualSprintToggled;

        public bool IsJumpTriggered
        {
            get
            {
                if (_virtualJumpTriggered)
                {
                    _virtualJumpTriggered = false;
                    return true;
                }
                return Input.GetKeyDown(KeyCode.Space);
            }
        }

        public bool IsInteractTriggered
        {
            get
            {
                if (_virtualInteractTriggered)
                {
                    _virtualInteractTriggered = false;
                    return true;
                }
                return Input.GetKeyDown(KeyCode.E);
            }
        }

        public bool IsPhotoModeTriggered => Input.GetKeyDown(KeyCode.P);

        public Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public void Shutdown()
        {
            IsInitialized = false;
        }

        public void SetVirtualMoveInput(Vector2 move) => _virtualMove = move;
        public void SetVirtualLookInput(Vector2 look) => _virtualLook = look;
        public void TriggerVirtualJump() => _virtualJumpTriggered = true;
        public void TriggerVirtualInteract() => _virtualInteractTriggered = true;
        public void ToggleVirtualSprint() => _virtualSprintToggled = !_virtualSprintToggled;
    }
}
