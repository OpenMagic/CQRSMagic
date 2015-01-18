using System;

namespace CQRSMagic.WebApiExample.Products.Commands
{
    public class DeleteProductCommand : Command
    {
        public DeleteProductCommand(Guid id) : base(id)
        {
        }
    }
}