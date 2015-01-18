using System;

namespace CQRSMagic.WebApiExample.Products
{
    public class ProductReadModel
    {
        // HttpContent.ReadAsAsync<T>() requires a public parameterless constructor.
        public ProductReadModel()
        {
        }

        public ProductReadModel(ProductEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            UnitPrice = entity.UnitPrice;
        }

        // todo: Change to private setters. Current public because HttpContent.ReadAsAsync<T>() requires public setters.
        public Guid Id { get; set;  }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }
}