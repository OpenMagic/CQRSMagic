﻿using System;

namespace CQRSMagic.WebApiExample.Products.Events
{
    public class AddedProductEvent : IEvent
    {
        private readonly Guid _id;
        private readonly string _name;
        private readonly decimal _unitPrice;

        public AddedProductEvent(Guid id, string name, decimal unitPrice)
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