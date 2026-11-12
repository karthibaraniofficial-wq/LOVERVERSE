using UnityEngine;
using UnityEngine.EventSystems;
using Loveverse.Core;
using Loveverse.Movement;

namespace Loveverse.UI
{
    /// <summary>
    /// Floating / dynamic virtual analog joystick for Android touch navigation.
    /// </summary>
    public sealed class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;
        [SerializeField] private float _handleRange = 64f;

        private Vector2 _inputVector;
        private IInputService _inputService;

        private void Start()
        {
            if (ServiceRegistry.Instance.TryResolve<IInputService>(out var input))
            {
                _inputService = input;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_background == null || _handle == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _background,
                eventData.position,
                eventData.pressEventCamera,
                out var position
            );

            position.x = (position.x / _background.sizeDelta.x);
            position.y = (position.y / _background.sizeDelta.y);

            _inputVector = new Vector2(position.x * 2f, position.y * 2f);
            _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

            float radius = _handleRange > 0f ? _handleRange : (_background.sizeDelta.x * 0.5f);
            _handle.anchoredPosition = new Vector2(
                _inputVector.x * radius,
                _inputVector.y * radius
            );

            _inputService?.SetVirtualMoveInput(_inputVector);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _inputVector = Vector2.zero;
            if (_handle != null)
            {
                _handle.anchoredPosition = Vector2.zero;
            }
            _inputService?.SetVirtualMoveInput(Vector2.zero);
        }
    }
}
