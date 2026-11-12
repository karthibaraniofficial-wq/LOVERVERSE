using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Loveverse.Core;

namespace Loveverse.Tests
{
    public class ServiceRegistryTests
    {
        private class MockGameService : IGameService
        {
            public bool IsInitialized { get; private set; }
            public Task InitializeAsync()
            {
                IsInitialized = true;
                return Task.CompletedTask;
            }
            public void Shutdown()
            {
                IsInitialized = false;
            }
        }

        [SetUp]
        public void Setup()
        {
            ServiceRegistry.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            ServiceRegistry.Reset();
        }

        [Test]
        public void RegisterAndResolve_ServiceReturnsCorrectInstance()
        {
            var registry = ServiceRegistry.Instance;
            var service = new MockGameService();

            registry.Register<IGameService>(service);
            var resolved = registry.Resolve<IGameService>();

            Assert.IsNotNull(resolved);
            Assert.AreSame(service, resolved);
        }

        [Test]
        public void Resolve_UnregisteredType_ThrowsKeyNotFoundException()
        {
            var registry = ServiceRegistry.Instance;
            Assert.Throws<KeyNotFoundException>(() => registry.Resolve<IGameService>());
        }

        [Test]
        public void TryResolve_ReturnsFalse_WhenNotRegistered()
        {
            var registry = ServiceRegistry.Instance;
            bool success = registry.TryResolve<IGameService>(out var resolved);

            Assert.IsFalse(success);
            Assert.IsNull(resolved);
        }

        [Test]
        public void Unregister_RemovesService()
        {
            var registry = ServiceRegistry.Instance;
            var service = new MockGameService();

            registry.Register<IGameService>(service);
            registry.Unregister<IGameService>();

            Assert.IsFalse(registry.TryResolve<IGameService>(out _));
        }
    }
}
