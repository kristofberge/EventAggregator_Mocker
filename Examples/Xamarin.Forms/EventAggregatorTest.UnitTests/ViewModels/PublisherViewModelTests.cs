using EventAggregatorMocker;
using EventAggregatorTest.Events;
using EventAggregatorTest.Viewmodels;
using Moq;
using Prism.Events;
using Xunit;

namespace EventAggregatorTest.UnitTests.ViewModels
{
    public class PublisherViewModelTests
    {
        private PublisherViewModel _viewModel;

        private readonly Mock<IEventAggregator> _eventAggregator;
        private readonly Mock<MessageSubmittedEvent> _mockedEvent;

        public PublisherViewModelTests()
        {
            _eventAggregator = new Mock<IEventAggregator>();

            _mockedEvent = _eventAggregator.RegisterNewMockedEvent<MessageSubmittedEvent, MessageContent>();

            _viewModel = new PublisherViewModel(_eventAggregator.Object);
        }

        [Fact]
        public void PublishEvent()
        {
            // Setup
            _viewModel.Text = "This is the text";
            _viewModel.Number = 4;

            // Act
            _viewModel.PublishCommand.Execute(null);

            // Verify
            _mockedEvent.Verify(x => x.Publish(It.Is<MessageContent>(m => m.Text == "This is the text" && m.Number == 4)));
        }
    }
}
