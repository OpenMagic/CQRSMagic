﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSMagic.Domain;
using CQRSMagic.Event;

namespace CQRSMagic.EventStorage
{
    public interface IEventStore
    {
        Task<IEnumerable<IEvent>> FindAllEventsAsync();
        Task<IEnumerable<IEvent>> FindEventsAsync(Guid aggregateId);

        TAggregate GetAggregate<TAggregate>(IEnumerable<IEvent> events) where TAggregate : IAggregate;

        Task<TAggregate> GetAggregateAsync<TAggregate>(Guid aggregateId) where TAggregate : IAggregate;

        Task SaveEventsAsync(IEnumerable<IEvent> events);
    }
}