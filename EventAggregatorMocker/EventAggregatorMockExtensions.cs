using System;
using Moq;
using Prism.Events;

namespace EventAggregatorMocker
{
    public static class EventAggregatorMockExtensions
    {
        public static Mock<TEvent> RegisterNewMockedEvent<TEvent>(this Mock<IEventAggregator> eventAggregator) where TEvent : PubSubEvent, new()
        {
            var mockedEvent = new Mock<TEvent>();
            eventAggregator.Setup(x => x.GetEvent<TEvent>()).Returns(mockedEvent.Object);

            return mockedEvent;
        }

        public static Mock<TEvent> RegisterNewMockedEvent<TEvent, TResult>(this Mock<IEventAggregator> eventAggregator) where TEvent : PubSubEvent<TResult>, new()
        {
            var mockedEvent = new Mock<TEvent>();
            eventAggregator.Setup(x => x.GetEvent<TEvent>()).Returns(mockedEvent.Object);

            return mockedEvent;
        }

        public static void SetSubscribeCallback<TEvent>(
            this Mock<TEvent> @event,
            Action<Action> onSubscribe,
            ThreadOption threadOption = default,
            bool keepSubcriberAlive = false) where TEvent : PubSubEvent
        {
            @event.Setup(x =>
                x.Subscribe(
                    It.IsAny<Action>(),
                    threadOption,
                    keepSubcriberAlive))
                .Callback<Action, ThreadOption, bool>(
                    (action, _, __) => onSubscribe(action));
        }

        public static void SetSubscribeCallback<TEvent, TResult>(
            this Mock<TEvent> @event, 
            Action<Action<TResult>> onSubscribe,
            ThreadOption threadOption = default,
            bool keepSubcriberAlive = false) where TEvent : PubSubEvent<TResult>
        {
            @event.Setup(x =>
                x.Subscribe(
                    It.IsAny<Action<TResult>>(),
                    threadOption,
                    keepSubcriberAlive,
                    It.IsAny<Predicate<TResult>>()))
                .Callback<Action<TResult>, ThreadOption, bool, Predicate<TResult>>(
                    (action, _, __, ___) => onSubscribe(action));
        }
    }
}
