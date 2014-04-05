namespace CQRSMagic.Specifications.Support.Fakes
{
    public class FakeEventStore : EventStore
    {
        public FakeEventStore() : base(new FakeEventStoreRepository())
        {
        }
    }
}