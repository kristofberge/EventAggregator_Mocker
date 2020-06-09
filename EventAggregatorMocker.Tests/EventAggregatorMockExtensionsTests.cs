using System;
using System.Collections;
using System.Collections.Generic;
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
        public void RegisterNewMockedEvent_ReturnsMockedEvent_NoParam()
        {
            Mock<DummyEvent> mockedEvent = _eventAggregator.RegisterNewMockedEvent<DummyEvent>();

            Assert.NotNull(mockedEvent);
        }

        [Fact]
        public void RegisterNewMockedEvent_ReturnsMockedEvent_WithParam()
        {
            Mock<DummyParamEvent> mockedEvent = _eventAggregator.RegisterNewMockedEvent<DummyParamEvent, object>();

            Assert.NotNull(mockedEvent);
        }

        [Fact]
        public void RegisterNewMockedEvent_SetsUpGetEvent_NoParam()
        {
            _ = _eventAggregator.RegisterNewMockedEvent<DummyEvent>();

            DummyEvent @event = _eventAggregator.Object.GetEvent<DummyEvent>();

            Assert.NotNull(@event);
        }

        [Fact]
        public void RegisterNewMockedEvent_SetsUpGetEvent_WithParam()
        {
            _ = _eventAggregator.RegisterNewMockedEvent<DummyParamEvent, object>();

            DummyParamEvent @event = _eventAggregator.Object.GetEvent<DummyParamEvent>();

            Assert.NotNull(@event);
        }

        [Fact]
        public void SetSubscribeCallback_PassesActionToSubscribeCallback_NoParam()
        {
            var expectedAction = new Action(() => { });
            Action receivedEvent = null;

            _eventAggregator
                .RegisterNewMockedEvent<DummyEvent>()
                .SetSubscribeCallback(action => receivedEvent = action);

            _eventAggregator.Object
                .GetEvent<DummyEvent>()
                .Subscribe(expectedAction);

            Assert.Equal(expectedAction, receivedEvent);
        }

        [Fact]
        public void SetSubscribeCallback_PassesActionToSubscribeCallback_WithParam()
        {
            var expectedAction = new Action<object>(obj => { });
            Action<object> receivedEvent = null;

            _eventAggregator
                .RegisterNewMockedEvent<DummyParamEvent, object>()
                .SetSubscribeCallback<DummyParamEvent, object>(action => receivedEvent = action);

            _eventAggregator.Object
                .GetEvent<DummyParamEvent>()
                .Subscribe(expectedAction);

            Assert.Equal(expectedAction, receivedEvent);
        }

        [Theory]
        [ClassData(typeof(OptionalParameters))]
        public void SetSubscribeCallback_PassesActionToSubscribeCallback_NoParam_Full(ThreadOption threadOption, bool keepSubcriberAlive)
        {
            var expectedAction = new Action(() => { });
            Action receivedAction = null;

            _eventAggregator
                .RegisterNewMockedEvent<DummyEvent>()
                .SetSubscribeCallback(action => receivedAction = action, threadOption, keepSubcriberAlive);

            _eventAggregator.Object
                .GetEvent<DummyEvent>()
                .Subscribe(expectedAction, threadOption, keepSubcriberAlive);

            Assert.Equal(expectedAction, receivedAction);
        }

        [Theory]
        [ClassData(typeof(OptionalParameters))]
        public void SetSubscribeCallback_PassesActionToSubscribeCallback_WithParam_Full(ThreadOption threadOption, bool keepSubcriberAlive)
        {
            var expectedAction = new Action<object>(obj => { });
            var filter = new Predicate<object>(obj => true);
            Action<object> receivedAction = null;

            _eventAggregator
                .RegisterNewMockedEvent<DummyParamEvent, object>()
                .SetSubscribeCallback(action => receivedAction = action, threadOption, keepSubcriberAlive, filter);

            _eventAggregator.Object
                .GetEvent<DummyParamEvent>()
                .Subscribe(expectedAction, threadOption, keepSubcriberAlive, filter);

            Assert.Equal(expectedAction, receivedAction);
        }

        private class OptionalParameters : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var threadOptionArray = new ThreadOption[] { ThreadOption.BackgroundThread, ThreadOption.PublisherThread, ThreadOption.UIThread };

                foreach(var threadOption in threadOptionArray)
                {
                    yield return new object[] { threadOption, true };
                    yield return new object[] { threadOption, false };
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
