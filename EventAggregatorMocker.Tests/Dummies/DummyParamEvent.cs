using Prism.Events;

namespace EventAggregatorMocker.Tests.Dummies
{
    public class DummyParamEvent : PubSubEvent<object>
    {
        public DummyParamEvent()
        {
        }
    }
}
