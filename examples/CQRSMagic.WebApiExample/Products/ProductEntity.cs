using CQRSMagic.WebApiExample.Products.Events;

namespace CQRSMagic.WebApiExample.Products
{
    public class ProductEntity : Entity
    {
        public string Name { get; private set; }
        public decimal UnitPrice { get; private set; }

        // ReSharper disable once UnusedMember.Local
        private void ApplyEvent(AddedProductEvent @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            UnitPrice = @event.UnitPrice;
        }
    }
}