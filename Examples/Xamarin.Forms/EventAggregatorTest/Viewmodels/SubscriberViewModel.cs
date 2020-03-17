using EventAggregatorTest.Events;
using Prism.Events;
using Prism.Mvvm;

namespace EventAggregatorTest.Viewmodels
{
    public class SubscriberViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private string _message;
        private int _number;

        public SubscriberViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator
                .GetEvent<MessageSubmittedEvent>()
                .Subscribe(OnMessageSubmitted);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public int Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        private void OnMessageSubmitted(MessageContent message)
        {
            Message = message.Text;
            Number = message.Number;
        }
    }
}
