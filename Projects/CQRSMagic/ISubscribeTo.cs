using System;

namespace Library.CQRS
{
    public interface ISubscribeTo<in TEvent>
    {
        void Handle(Guid aggregateId, TEvent e);
    }
}