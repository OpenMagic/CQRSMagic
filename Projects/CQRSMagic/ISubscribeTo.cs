using System;

namespace CQRSMagic
{
    public interface ISubscribeTo<in TEvent>
    {
        void Handle(Guid aggregateId, TEvent e);
    }
}