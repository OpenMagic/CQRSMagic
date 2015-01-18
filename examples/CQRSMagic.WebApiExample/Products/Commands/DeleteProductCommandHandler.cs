using System.Collections.Generic;
using CQRSMagic.WebApiExample.Products.Events;

namespace CQRSMagic.WebApiExample.Products.Commands
{
    public class DeleteProductCommandHandler
    {
        private readonly IEventStore _eventStore;

        public DeleteProductCommandHandler() : this(ServiceLocator.EventStore)
        {
        }

        public DeleteProductCommandHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public IEnumerable<IEvent> Handle(DeleteProductCommand command)
        {
            var entity = _eventStore.GetEntity<ProductEntity>(command.Id);

            if (entity == null)
            {
                throw new EntityNotFoundException<ProductEntity>(command.Id);
            }

            yield return new DeletedProductEvent(command.Id);
        }
    }
}