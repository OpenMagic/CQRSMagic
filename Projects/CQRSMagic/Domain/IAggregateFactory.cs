namespace CQRSMagic.Domain
{
    public interface IAggregateFactory
    {
        TAggregate CreateInstance<TAggregate>() where TAggregate : IAggregate;
    }
}