using System;
using EventAggregatorMocker.Tests.Dummies;
using Moq;
using Prism.Events;
using Xunit;

namespace EventAggregatorMocker.Tests
{
    public class EventAggregatorMockExtensionsTests
    {
        Mock<IEventAggregator> _eventAggregator;

        public EventAggregatorMockExtensionsTests()
        {
            _eventAggregator = new Mock<IEventAggregator>();
        }

        [Fact]
        public void ReturnsMockedEventNoParam()
        {
            Mock<DummyEvent> mockedEvent = _eventAggregator.RegisterNewMockedEvent<DummyEvent>();

            Assert.NotNull(mockedEvent);
        }

        [Fact]
        public void ReturnsMockedEventWithParam()
        {
            Mock<DummyParamEvent> mockedEvent = _eventAggregator.RegisterNewMockedEvent<DummyParamEvent, object>();

            Assert.NotNull(mockedEvent);
        }

        [Fact]
        public void GetEventSuccessfullySetupNoParam()
        {
            _ = _eventAggregator.RegisterNewMockedEvent<DummyEvent>();

            DummyEvent @event = _eventAggregator.Object.GetEvent<DummyEvent>();

            Assert.NotNull(@event);
        }

        [Fact]
        public void GetEventSuccessfullySetupWithParam()
        {
            _ = _eventAggregator.RegisterNewMockedEvent<DummyParamEvent, object>();

            DummyParamEvent @event = _eventAggregator.Object.GetEvent<DummyParamEvent>();

            Assert.NotNull(@event);
        }

        [Fact]
        public void SubscribeActionPassedToCallbackNoParam()
        {
            var expectedAction = new Action(() => { });
            Action receivedEvent = null;

            _ = _eventAggregator.RegisterNewMockedEvent<DummyEvent>(action => receivedEvent = action);

            _eventAggregator.Object
                .GetEvent<DummyEvent>()
                .Subscribe(expectedAction);

            Assert.Equal(expectedAction, receivedEvent);
        }

        [Fact]
        public void SubscribeActionPassedToCallbackWithParam()
        {
            var expectedAction = new Action<object>(obj => { });
            Action<object> receivedEvent = null;

            _ = _eventAggregator.RegisterNewMockedEvent<DummyParamEvent, object>(action => receivedEvent = action);

            _eventAggregator.Object
                .GetEvent<DummyParamEvent>()
                .Subscribe(expectedAction);

            Assert.Equal(expectedAction, receivedEvent);
        }
    }
}
