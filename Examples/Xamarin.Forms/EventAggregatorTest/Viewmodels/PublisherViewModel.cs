using System.Windows.Input;
using EventAggregatorTest.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace EventAggregatorTest.Viewmodels
{
    public class PublisherViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private string _text;
        private int _number;
        private ICommand _addCommand;
        private ICommand _subtractCommand;
        private ICommand _publishCommand;

        public PublisherViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            AddCommand = new DelegateCommand(AddNumber);
            SubtractCommand = new DelegateCommand(SubtractNumber);
            PublishCommand = new DelegateCommand(PublishEvent);
        }

        private void AddNumber() => Number++;

        private void SubtractNumber() => Number--;

        private void PublishEvent()
        {
            var message = new MessageContent
            {
                Text = Text,
                Number = Number
            };

            _eventAggregator
                .GetEvent<MessageSubmittedEvent>()
                .Publish(message);
        }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public int Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        public ICommand AddCommand
        {
            get => _addCommand;
            set => SetProperty(ref _addCommand, value);
        }

        public ICommand SubtractCommand
        {
            get => _subtractCommand;
            set => SetProperty(ref _subtractCommand, value);
        }

        public ICommand PublishCommand
        {
            get => _publishCommand;
            set => SetProperty(ref _publishCommand, value);
        }
    }
}
