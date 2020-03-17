using System;
using Moq;
using Prism.Events;

namespace EventAggregatorMocker
{
    public static class EventAggregatorMockExtensions
    {
        public static Mock<TEvent> RegisterNewMockedEvent<TEvent>(this Mock<IEventAggregator> eventAggregator, Action<Action> onSubscribeAction = null) where TEvent : PubSubEvent, new()
        {
            var mockedEvent = new Mock<TEvent>();
            eventAggregator.Setup(x => x.GetEvent<TEvent>()).Returns(mockedEvent.Object);

            if (onSubscribeAction != null)
            {
                mockedEvent.SetCallback(onSubscribeAction);
            }
            return mockedEvent;
        }

        public static Mock<TEvent> RegisterNewMockedEvent<TEvent, TResult>(this Mock<IEventAggregator> eventAggregator, Action<Action<TResult>> onSubscribeAction = null) where TEvent : PubSubEvent<TResult>, new()
        {
            var mockedEvent = new Mock<TEvent>();
            eventAggregator.Setup(x => x.GetEvent<TEvent>()).Returns(mockedEvent.Object);

            if (onSubscribeAction != null)
            {
                mockedEvent.SetCallback(onSubscribeAction);
            }
            return mockedEvent;
        }

        public static void SetCallback<TEvent>(this Mock<TEvent> @event, Action<Action> onSubscribe) where TEvent : PubSubEvent
        {
            @event.Setup(x =>
                x.Subscribe(
                    It.IsAny<Action>(),
                    It.IsAny<ThreadOption>(),
                    It.IsAny<bool>()))
                .Callback<Action, ThreadOption, bool>(
                    (action, _, __) => onSubscribe(action));
        }

        public static void SetCallback<TResult, TEvent>(this Mock<TEvent> @event, Action<Action<TResult>> onSubscribe) where TEvent : PubSubEvent<TResult>
        {
            @event.Setup(x =>
                x.Subscribe(
                    It.IsAny<Action<TResult>>(),
                    It.IsAny<ThreadOption>(),
                    It.IsAny<bool>(),
                    It.IsAny<Predicate<TResult>>()))
                .Callback<Action<TResult>, ThreadOption, bool, Predicate<TResult>>(
                    (action, _, __, ___) => onSubscribe(action));
        }
    }
}
