using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CQRSMagic.WebApiExample.Products
{
    public class ProductsController : ApiController
    {
        public IEnumerable<ProductReadModel> Get()
        {
            throw new NotImplementedException();
        }

        public ProductReadModel Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Post(string name, decimal unitPrice)
        {
            throw new NotImplementedException();
        }

        public void Put(Guid id, string name, decimal unitPrice)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}