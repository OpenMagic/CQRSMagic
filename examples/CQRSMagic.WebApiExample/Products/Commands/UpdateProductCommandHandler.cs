using System.Collections.Generic;
using CQRSMagic.WebApiExample.Products.Events;

namespace CQRSMagic.WebApiExample.Products.Commands
{
    public class UpdateProductCommandHandler
    {
        private readonly IEventStore _eventStore;

        public UpdateProductCommandHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public IEnumerable<IEvent> Handle(UpdateProductCommand command)
        {
            var entity = _eventStore.GetEntity<ProductEntity>(command.Id);

            if (entity == null)
            {
                throw new EntityNotFoundException<ProductEntity>(command.Id);
            }

            if (entity.Name != command.Name)
            {
                yield return new ProductNameChangedEvent(command.Id, command.Name);
            }

            if (entity.UnitPrice != command.UnitPrice)
            {
                yield return new ProductUnitPriceChangedEvent(command.Id, command.UnitPrice);
            }
        }
    }
}