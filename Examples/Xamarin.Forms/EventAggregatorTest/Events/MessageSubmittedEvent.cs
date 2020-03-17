using Prism.Events;

namespace EventAggregatorTest.Events
{
    public class MessageSubmittedEvent : PubSubEvent<MessageContent>
    {
    }

    public class MessageContent
    {
        public string Text { get; set; }
        public int Number { get; set; }
    }
}
