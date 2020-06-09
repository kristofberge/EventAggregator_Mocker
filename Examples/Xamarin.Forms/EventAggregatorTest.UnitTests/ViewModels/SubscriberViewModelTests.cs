using System;
using EventAggregatorMocker;
using EventAggregatorTest.Events;
using EventAggregatorTest.Viewmodels;
using Moq;
using Prism.Events;
using Xunit;

namespace EventAggregatorTest.UnitTests.ViewModels
{
    public class SubscriberViewModelTests
    {
        private SubscriberViewModel _viewModel;

        private readonly Mock<IEventAggregator> _eventAggregator;
        private Action<MessageContent> _onMessageSubmitted;

        public SubscriberViewModelTests()
        {
            _eventAggregator = new Mock<IEventAggregator>();

            _eventAggregator
                .RegisterNewMockedEvent<MessageSubmittedEvent, MessageContent>()
                .SetSubscribeCallback<MessageSubmittedEvent, MessageContent>(action => _onMessageSubmitted = action);

            _viewModel = new SubscriberViewModel(_eventAggregator.Object);
        }

        [Fact]
        public void FieldsUpdatedAfterMessageReceived()
        {
            //Setup
            var message = new MessageContent
            {
                Text = "Expected text",
                Number = 4
            };

            //Act
            _onMessageSubmitted.Invoke(message);

            //Verify
            Assert.Equal("Expected text", _viewModel.Message);
            Assert.Equal(4, _viewModel.Number);
        }
    }
}