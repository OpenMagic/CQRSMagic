using System;

namespace CQRSMagic.WebApiExample.Products.Events
{
    public class DeletedProductEvent : IEvent
    {
        private readonly Guid _id;

        public DeletedProductEvent(Guid id)
        {
            _id = id;
        }

        public Guid Id { get { return _id; } }
    }
}