using System;

namespace CQRSMagic.WebApiExample.Products.Commands
{
    public class UpdateProductCommand : Command
    {
        private readonly string _name;
        private readonly decimal _unitPrice;

        public UpdateProductCommand(Guid id, string name, decimal unitPrice)
            : base(id)
        {
            _name = name;
            _unitPrice = unitPrice;
        }

        public string Name { get { return _name; } }
        public decimal UnitPrice { get { return _unitPrice; } }
    }
}