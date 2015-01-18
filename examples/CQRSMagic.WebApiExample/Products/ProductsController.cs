using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CQRSMagic.WebApiExample.Products.Commands;
using Newtonsoft.Json.Linq;

namespace CQRSMagic.WebApiExample.Products
{
    public class ProductsController : ApiController
    {
        private readonly IEventStore _eventStore;
        private readonly IEventPublisher _eventPublisher;

        public ProductsController()
            : this(ServiceLocator.EventStore, ServiceLocator.EventPublisher)
        {
        }

        public ProductsController(IEventStore eventStore, IEventPublisher eventPublisher)
        {
            _eventStore = eventStore;
            _eventPublisher = eventPublisher;
        }

        public IEnumerable<ProductReadModel> Get()
        {
            return ServiceLocator.ProductReadModels.AsEnumerable();
        }

        public ProductReadModel Get(Guid id)
        {
            var product = ServiceLocator.ProductReadModels.SingleOrDefault(p => p.Id == id);

            return product;
        }

        public HttpResponseMessage Post(JObject value)
        {
            var name = value.Value<string>("name");
            var unitPrice = value.Value<decimal>("unitPrice");
            var command = new AddProductCommand(name, unitPrice);

            var commandHandler = new AddProductCommandHandler();
            var events = commandHandler.Handle(command).ToArray();

            _eventStore.SaveEvents(command.Id, 0, events);
            _eventPublisher.PublishEvents(events);

            var response = Request.CreateResponse(HttpStatusCode.Created, new { command.Id, name, unitPrice });

            var uri = Url.Link("DefaultApi", new { id = command.Id });
            response.Headers.Location = new Uri(uri);

            return response;
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