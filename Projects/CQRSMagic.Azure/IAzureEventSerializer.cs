﻿using CQRSMagic.Event;
using Microsoft.WindowsAzure.Storage.Table;

namespace CQRSMagic.Azure
{
    public interface IAzureEventSerializer
    {
        IEvent Deserialize(DynamicTableEntity entity);
        DynamicTableEntity Serialize(IEvent @event);
    }
}