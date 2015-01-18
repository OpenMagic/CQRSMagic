using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRSMagic.WebApiExample.Products
{
    public class ProductReadModelRepository : IProductReadModelRepository
    {
        private static readonly Dictionary<Guid, ProductReadModel> Products = new Dictionary<Guid, ProductReadModel>();

        public void Add(Guid id, ProductReadModel readModel)
        {
            Products.Add(id, readModel);
        }

        public void Delete(Guid id)
        {
            Products.Remove(id);
        }

        public void Update(ProductReadModel readModel)
        {
            Products[readModel.Id] = readModel;
        }

        public ProductReadModel[] GetAll()
        {
            return Products.Values.ToArray();
        }

        public ProductReadModel GetById(Guid id)
        {
            return Products[id];
        }
    }
}