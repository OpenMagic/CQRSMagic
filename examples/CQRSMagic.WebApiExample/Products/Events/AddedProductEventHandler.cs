using System.Collections.Generic;

namespace CQRSMagic.WebApiExample.Products.Events
{
    public class AddedProductEventHandler
    {
        private readonly IList<ProductReadModel> _products;
        private readonly IEventStore _eventStore;

        public AddedProductEventHandler() : this(ServiceLocator.ProductReadModels, ServiceLocator.EventStore)
        {
        }

        public AddedProductEventHandler(IList<ProductReadModel> products, IEventStore eventStore)
        {
            _products = products;
            _eventStore = eventStore;
        }

        public void Handle(AddedProductEvent addedProductEvent)
        {
            var entity = _eventStore.GetEntity<ProductEntity>(addedProductEvent.Id);
            var readModel = new ProductReadModel(entity);

            _products.Add(readModel);
        }
    }
}