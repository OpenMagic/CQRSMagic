using System;

namespace CQRSMagic.WebApiExample.Products.Commands
{
    public class AddProductCommand : Command
    {
        private readonly string _name;
        private readonly decimal _unitPrice;

        public AddProductCommand(string name, decimal unitPrice)
            : base(Guid.NewGuid())
        {
            _name = name;
            _unitPrice = unitPrice;
        }

        public string Name { get { return _name; } }
        public decimal UnitPrice { get { return _unitPrice; } }
    }
}