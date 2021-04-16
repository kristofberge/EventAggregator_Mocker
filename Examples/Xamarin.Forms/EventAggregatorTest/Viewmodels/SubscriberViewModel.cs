using System;
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
        private string _messageFiltered;
        private int _numberFiltered;

        public SubscriberViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator
                .GetEvent<MessageSubmittedEvent>()
                .Subscribe(OnMessageReceived);


            _eventAggregator
                .GetEvent<MessageSubmittedEvent>()
                .Subscribe(OnFilteredMessageReceived, filter: msg => msg.Number > 4);
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

        public string MessageFiltered
        {
            get => _messageFiltered;
            set => SetProperty(ref _messageFiltered, value);
        }

        public int NumberFiltered
        {
            get => _numberFiltered;
            set => SetProperty(ref _numberFiltered, value);
        }

        private void OnMessageReceived(MessageContent message)
        {
            Message = message.Text;
            Number = message.Number;
        }

        private void OnFilteredMessageReceived(MessageContent message)
        {
            MessageFiltered = message.Text;
            NumberFiltered = message.Number;
        }
    }
}
