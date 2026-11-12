using NUnit.Framework;
using Loveverse.Core;

namespace Loveverse.Tests
{
    public class EventBusTests
    {
        private struct TestSampleEvent
        {
            public int Value;
        }

        [SetUp]
        public void Setup()
        {
            EventBus.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            EventBus.Clear();
        }

        [Test]
        public void SubscribeAndPublish_InvokesCallbackWithCorrectPayload()
        {
            int receivedValue = 0;
            EventBus.Subscribe<TestSampleEvent>(e => receivedValue = e.Value);

            EventBus.Publish(new TestSampleEvent { Value = 42 });

            Assert.AreEqual(42, receivedValue);
        }

        [Test]
        public void Unsubscribe_StopsCallbackInvocation()
        {
            int invocationCount = 0;
            void Handler(TestSampleEvent e) => invocationCount++;

            EventBus.Subscribe<TestSampleEvent>(Handler);
            EventBus.Publish(new TestSampleEvent { Value = 1 });
            Assert.AreEqual(1, invocationCount);

            EventBus.Unsubscribe<TestSampleEvent>(Handler);
            EventBus.Publish(new TestSampleEvent { Value = 2 });
            Assert.AreEqual(1, invocationCount, "Callback should not be called after unsubscription");
        }
    }
}
