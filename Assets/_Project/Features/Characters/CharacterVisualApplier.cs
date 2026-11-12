using System.Collections.Generic;
using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Characters
{
    /// <summary>
    /// Applies CharacterAppearance customization data to the character's skinned mesh renderers and materials.
    /// </summary>
    public sealed class CharacterVisualApplier : MonoBehaviour
    {
        [Header("Renderers & Sockets")]
        [SerializeField] private SkinnedMeshRenderer _bodyRenderer;
        [SerializeField] private Transform _hairSocket;
        [SerializeField] private Transform _accessorySocket;

        [Header("Material Indices")]
        [SerializeField] private int _skinMaterialIndex = 0;
        [SerializeField] private int _eyeMaterialIndex = 1;

        private GameObject _spawnedHair;
        private GameObject _spawnedAccessory;

        public void ApplyAppearance(CharacterAppearance appearance, Dictionary<string, CustomizationItem> itemDatabase = null)
        {
            if (appearance == null) return;

            // Apply Skin and Eye Colors via MaterialPropertyBlock to prevent breaking dynamic batching
            if (_bodyRenderer != null)
            {
                var mpb = new MaterialPropertyBlock();
                _bodyRenderer.GetPropertyBlock(mpb, _skinMaterialIndex);
                mpb.SetColor("_BaseColor", appearance.GetSkinColor());
                _bodyRenderer.SetPropertyBlock(mpb, _skinMaterialIndex);

                _bodyRenderer.GetPropertyBlock(mpb, _eyeMaterialIndex);
                mpb.SetColor("_BaseColor", appearance.GetEyeColor());
                _bodyRenderer.SetPropertyBlock(mpb, _eyeMaterialIndex);
            }

            // Spawn and tint hair
            if (itemDatabase != null && itemDatabase.TryGetValue(appearance.HairStyleId, out var hairItem) && hairItem.Prefab != null)
            {
                if (_spawnedHair != null) Destroy(_spawnedHair);
                _spawnedHair = Instantiate(hairItem.Prefab, _hairSocket != null ? _hairSocket : transform);
                _spawnedHair.transform.localPosition = Vector3.zero;
                _spawnedHair.transform.localRotation = Quaternion.identity;

                var hairRenderer = _spawnedHair.GetComponentInChildren<Renderer>();
                if (hairRenderer != null)
                {
                    var mpb = new MaterialPropertyBlock();
                    hairRenderer.GetPropertyBlock(mpb);
                    mpb.SetColor("_BaseColor", appearance.GetHairColor());
                    hairRenderer.SetPropertyBlock(mpb);
                }
            }
        }
    }
}
