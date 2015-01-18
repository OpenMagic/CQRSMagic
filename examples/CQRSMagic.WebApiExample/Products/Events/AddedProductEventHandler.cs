using System.Collections.Generic;

namespace CQRSMagic.WebApiExample.Products.Events
{
    public class AddedProductEventHandler
    {
        private readonly IList<ProductReadModel> _products;

        public AddedProductEventHandler() : this(ServiceLocator.ProductReadModels)
        {
        }

        public AddedProductEventHandler(IList<ProductReadModel> products)
        {
            _products = products;
        }

        public void Handle(AddedProductEvent addedProductEvent)
        {
            _products.Add(new ProductReadModel(addedProductEvent.Id, addedProductEvent.Name, addedProductEvent.UnitPrice));
        }
    }
}