using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Movement
{
    public interface IInputService : IGameService
    {
        Vector2 MoveInput { get; }
        Vector2 LookInput { get; }
        bool IsSprintPressed { get; }
        bool IsJumpTriggered { get; }
        bool IsInteractTriggered { get; }
        bool IsPhotoModeTriggered { get; }

        void SetVirtualMoveInput(Vector2 move);
        void SetVirtualLookInput(Vector2 look);
        void TriggerVirtualJump();
        void TriggerVirtualInteract();
        void ToggleVirtualSprint();
    }
}
