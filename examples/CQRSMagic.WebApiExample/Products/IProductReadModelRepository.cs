using System;
using System.Collections.Generic;

namespace CQRSMagic.WebApiExample.Products
{
    public interface IProductReadModelRepository
    {
        void Add(Guid id, ProductReadModel readModel);
        void Delete(Guid id);
        void Update(ProductReadModel readModel);

        ProductReadModel[] GetAll();
        ProductReadModel GetById(Guid id);
    }
}