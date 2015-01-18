using System;

namespace CQRSMagic.WebApiExample.Products.Commands
{
    public class UpdateProductCommand : ICommand
    {
        private readonly Guid _id;
        private readonly string _name;
        private readonly decimal _unitPrice;

        public UpdateProductCommand(Guid id, string name, decimal unitPrice)
        {
            _id = id;
            _name = name;
            _unitPrice = unitPrice;
        }

        public Guid Id { get { return _id; } }
        public string Name { get { return _name; } }
        public decimal UnitPrice { get { return _unitPrice; } }
    }
}