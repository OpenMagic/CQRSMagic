using BoDi;
using CQRSMagic.Specifications.Support.Fakes;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Support.Bindings
{
    [Binding]
    public class EventStoreBinding
    {
        private readonly IObjectContainer Container;

        public EventStoreBinding(IObjectContainer container)
        {
            Container = container;
        }

        [BeforeScenario]
        public void InitializerContainer()
        {
            Container.RegisterTypeAs<EventStore, IEventStore>();
            Container.RegisterTypeAs<FakeEventStoreRepository, IEventStoreRepository>();
        }
    }
}