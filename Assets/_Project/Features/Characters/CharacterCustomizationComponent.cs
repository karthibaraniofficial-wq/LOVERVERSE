using UnityEngine;
using Loveverse.Core;

namespace Loveverse.Characters
{
    /// <summary>
    /// Lifecycle component that hosts and registers CharacterCustomizationService into ServiceRegistry.
    /// </summary>
    [DefaultExecutionOrder(-950)]
    public sealed class CharacterCustomizationComponent : MonoBehaviour
    {
        private async void Awake()
        {
            if (ServiceRegistry.Instance.TryResolve<ICharacterCustomizationService>(out _))
            {
                return;
            }

            var service = new CharacterCustomizationService();
            await service.InitializeAsync();
            ServiceRegistry.Instance.Register<ICharacterCustomizationService>(service);
            Lg.Info("CharacterCustomizationComponent registered ICharacterCustomizationService.");
        }
    }
}
