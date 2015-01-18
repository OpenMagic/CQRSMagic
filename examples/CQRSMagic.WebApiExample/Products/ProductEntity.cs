using CQRSMagic.WebApiExample.Products.Events;

namespace CQRSMagic.WebApiExample.Products
{
    public class ProductEntity : Entity
    {
        public string Name { get; private set; }
        public decimal UnitPrice { get; private set; }
        public bool IsDeleted { get; private set; }

        public void ApplyEvent(AddedProductEvent e)
        {
            Id = e.Id;
            Name = e.Name;
            UnitPrice = e.UnitPrice;
        }

        public void ApplyEvent(ProductNameChangedEvent e)
        {
            Name = e.Name;
        }

        public void ApplyEvent(ProductUnitPriceChangedEvent e)
        {
            UnitPrice = e.UnitPrice;
        }

        public void ApplyEvent(DeletedProductEvent e)
        {
            IsDeleted = true;
        }
    }
}