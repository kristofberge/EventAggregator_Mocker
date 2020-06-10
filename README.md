# EventAggregator_Mocker
EventAggregator_Mocker is a helper library for writing unit tests with Prism's EventAggregator and Moq. It allows you to verify that an event has been published with the correct parameters and that it was handled in the correct way.

[NuGet package](https://www.nuget.org/packages/EventAggregator_Mocker/)

You'll get extension methods for `Mock<IEventAggregator>` and `Mock<TEvent>` (where `TEvent` extends `PubSubEvent` or `PubSubEvent<T>`)
 
## How to use
In your unit test setup instantiate a mock of IEventAggregator.
 
`var eventAggregatorMock = new Mock<IEventAggregator>();`

### Verifying that .Publish() is actually called on the event
Call one of the extension methods on the IEventAggregator and store the returned mocked event in a variable.

Without parameter:
```
Mock<MyEvent> mockedEvent = eventAggregatorMock.RegisterNewMockedEvent<MyEvent>();
...
mockedEvent.Verify(evt => evt.Publish(), Times.Once);
```
With paramater:
```
Mock<MyParamEvent> mockedParamEvent = eventAggregatorMock.RegisterNewMockedEvent<MyParamEvent, MyParam>();
...
mockedParamEvent.Verify(evt => evt.Publish(It.IsAny<MyParam>), Times.Once);
```

### Verifying the event handling behavior
We need to get a reference to the event handler, so we can call it directly and verify its behavior. We do this by storing this reference in a variable of type `Action` or `Action<T>`.
The 2 extension methods I used above return an object of type `Mock<TEvent>`. On this object, you can call another extension method `SetSubscribeCallback<TEvent>(Action<Action> onSubscribe)`. This method takes an `Action` as a paramater that in its turn will recieve a reference to the event handler that's passed in the `Subscribe` method of the actual code you're testing. 
So at this point we can store this reference in a variable and invoke it in our unit test. This works even if the method passed is private.
If this sounds a bit complicated, here's an example:

Without parameter:
```
Action onEventPublishedAction;
// During setup
eventAggregatorMock
    .RegisterNewMockedEvent<MyEvent>()
    .SetSubscribeCallback<MyEvent>(action => onEventPublishedAction = action);
...
// In the unit test
onEventPublishedAction.Invoke();

// Verify that the code has run correctly
```

Without parameter:
```
Action<MyParam> onEventPublishedAction;
// During setup
eventAggregatorMock
    .RegisterNewMockedEvent<MyEvent, MyParam>()
    .SetSubscribeCallback<MyEvent, MyParam>(action => onEventPublishedAction = action);
...
// In the unit test
onEventPublishedAction.Invoke(new MyParam());

// Verify that the code has run correctly
```

To see it in action, check out this working [example](https://github.com/kristofberge/EventAggregator_Mocker/tree/master/Examples/Xamarin.Forms/EventAggregatorTest.UnitTests/ViewModels)  


## Optional parameters
Since version 1.0 EventAggregator_Mocker supports all optional parameters of `IEventAggragator.Subscriber()`.

For example:
```
eventAggregatorMock
    .RegisterNewMockedEvent<MyEvent>()
    .SetSubscribeCallback(
        action => receivedAction = action,
        ThreadOption.BackgroundThread,
        true
    );
```
