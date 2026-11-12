using UnityEngine;
using UnityEngine.UI;
using Loveverse.Core;
using Loveverse.Movement;

namespace Loveverse.UI
{
    public enum ActionButtonType
    {
        Jump,
        Sprint,
        Interact
    }

    [RequireComponent(typeof(Button))]
    public sealed class VirtualActionButton : MonoBehaviour
    {
        [SerializeField] private ActionButtonType _actionType;
        private IInputService _inputService;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            if (ServiceRegistry.Instance.TryResolve<IInputService>(out var input))
            {
                _inputService = input;
            }

            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (_inputService == null) return;

            switch (_actionType)
            {
                case ActionButtonType.Jump:
                    _inputService.TriggerVirtualJump();
                    break;
                case ActionButtonType.Sprint:
                    _inputService.ToggleVirtualSprint();
                    break;
                case ActionButtonType.Interact:
                    _inputService.TriggerVirtualInteract();
                    break;
            }
        }
    }
}
