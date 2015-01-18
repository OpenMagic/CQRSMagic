using System.Collections.Generic;
using CQRSMagic.WebApiExample.Products.Events;

namespace CQRSMagic.WebApiExample.Products.Commands
{
    public class AddProductCommandHandler
    {
        public IEnumerable<IEvent> Handle(AddProductCommand command)
        {
            yield return new AddedProductEvent(command.Id, command.Name, command.UnitPrice);
        }
    }
}