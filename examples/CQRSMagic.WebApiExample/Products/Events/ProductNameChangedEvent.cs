using System;

namespace CQRSMagic.WebApiExample.Products.Events
{
    public class ProductNameChangedEvent : IEvent
    {
        private readonly Guid _id;
        private readonly string _name;

        public ProductNameChangedEvent(Guid id, string name)
        {
            _id = id;
            _name = name;
        }

        public Guid Id { get { return _id; } }
        public string Name { get { return _name; } }
    }
}