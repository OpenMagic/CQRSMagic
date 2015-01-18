using System;

namespace CQRSMagic.WebApiExample.Products.Events
{
    public class UnitPriceChangedEvent : IEvent
    {
        private readonly Guid _id;
        private readonly decimal _unitPrice;

        public UnitPriceChangedEvent(Guid id, decimal unitPrice)
        {
            _id = id;
            _unitPrice = unitPrice;
        }

        public Guid Id { get { return _id; } }
        public decimal UnitPrice { get { return _unitPrice; } }
    }
}