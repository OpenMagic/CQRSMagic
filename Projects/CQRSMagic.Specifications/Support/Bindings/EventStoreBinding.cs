using BoDi;
using Library.CQRS.Specifications.Support.Fakes;
using TechTalk.SpecFlow;

namespace Library.CQRS.Specifications.Support.Bindings
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