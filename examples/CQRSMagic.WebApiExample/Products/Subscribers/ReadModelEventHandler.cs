using System;
using System.Collections.Generic;
using System.Linq;
using CQRSMagic.WebApiExample.Products.Events;

namespace CQRSMagic.WebApiExample.Products.Subscribers
{
    public class ReadModelEventHandler
    {
        private readonly Dictionary<Guid, ProductReadModel> _products;
        private readonly IEventStore _eventStore;

        public ReadModelEventHandler() : this(ServiceLocator.ProductReadModels, ServiceLocator.EventStore)
        {
        }

        public ReadModelEventHandler(Dictionary<Guid, ProductReadModel> products, IEventStore eventStore)
        {
            _products = products;
            _eventStore = eventStore;
        }

        public void Handle(AddedProductEvent addedProductEvent)
        {
            var readModel = GetReadModel(addedProductEvent.Id);

            _products.Add(readModel.Id, readModel);
        }

        public void Handle(NameChangedEvent nameChangedEvent)
        {
            UpdateReadModel(nameChangedEvent.Id);
        }

        public void Handle(UnitPriceChangedEvent unitPriceChangedEvent)
        {
            UpdateReadModel(unitPriceChangedEvent.Id);
        }

        private void UpdateReadModel(Guid id)
        {
            var readModel = GetReadModel(id);

            _products[id] = readModel;
        }

        private ProductReadModel GetReadModel(Guid productId)
        {
            var entity = _eventStore.GetEntity<ProductEntity>(productId);
            var readModel = new ProductReadModel(entity);

            return readModel;
        }

    }
}