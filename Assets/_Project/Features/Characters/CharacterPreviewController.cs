using UnityEngine;
using UnityEngine.EventSystems;

namespace Loveverse.Characters
{
    /// <summary>
    /// Controls 360-degree rotation and zoom for the character customization preview screen.
    /// </summary>
    public sealed class CharacterPreviewController : MonoBehaviour, IDragHandler
    {
        [SerializeField] private Transform _characterStage;
        [SerializeField] private float _rotationSpeed = 0.4f;

        public void OnDrag(PointerEventData eventData)
        {
            if (_characterStage == null) return;
            _characterStage.Rotate(Vector3.up, -eventData.delta.x * _rotationSpeed, Space.World);
        }

        public void ResetOrientation()
        {
            if (_characterStage != null)
            {
                _characterStage.rotation = Quaternion.identity;
            }
        }
    }
}
