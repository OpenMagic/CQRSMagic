using CQRSMagic.Event;
using Microsoft.WindowsAzure.Storage.Table;

namespace CQRSMagic.Azure
{
    public interface IAzureEventSerializer
    {
        int MaximumEventsPerTransaction { get;  }

        IEvent Deserialize(DynamicTableEntity entity);
        DynamicTableEntity Serialize(IEvent @event, int transactionIndex);
    }
}