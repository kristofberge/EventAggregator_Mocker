using System;
using System.Threading.Tasks;
using EventAggregatorTest.Viewmodels;
using EventAggregatorTest.Views;
using Prism;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Xamarin.Forms;

namespace EventAggregatorTest
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            try
            {
                TaskScheduler.UnobservedTaskException += (sender, e) => {
                    System.Diagnostics.Debug.WriteLine($"{e.Exception}");
                    var test = e.Exception;
                };
                await NavigationService.NavigateAsync("NavigationPage/MyTabbedPage");
            }
            catch (Exception e)
            {
                    System.Diagnostics.Debug.WriteLine($"{e}");
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MyTabbedPage, MyTabbedViewModel>();
            containerRegistry.RegisterForNavigation<PublisherPage, PublisherViewModel>();
            containerRegistry.RegisterForNavigation<SubscriberPage, SubscriberViewModel>();
        }
    }
}
