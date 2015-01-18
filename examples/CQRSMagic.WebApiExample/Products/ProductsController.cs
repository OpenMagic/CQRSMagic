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
        private readonly Dictionary<Guid, ProductReadModel> _productReadModels;

        public ProductsController()
            : this(ServiceLocator.EventStore, ServiceLocator.EventPublisher, ServiceLocator.ProductReadModels)
        {
        }

        public ProductsController(IEventStore eventStore, IEventPublisher eventPublisher, Dictionary<Guid, ProductReadModel> productReadModels)
        {
            _eventStore = eventStore;
            _eventPublisher = eventPublisher;
            _productReadModels = productReadModels;
        }

        public IEnumerable<ProductReadModel> Get()
        {
            return _productReadModels.Values.AsEnumerable();
        }

        public ProductReadModel Get(Guid id)
        {
            var product = _productReadModels[id];

            return product;
        }

        public HttpResponseMessage Post(JObject json)
        {
            var name = json.Value<string>("name");
            var unitPrice = json.Value<decimal>("unitPrice");
            var command = new AddProductCommand(name, unitPrice);

            // todo: refactor
            var commandHandler = new AddProductCommandHandler();
            var events = commandHandler.Handle(command).ToArray();

            _eventStore.SaveEvents(command.Id, 0, events);
            _eventPublisher.PublishEvents(events);

            var response = Request.CreateResponse(HttpStatusCode.Created, new { command.Id, name, unitPrice });

            var uri = Url.Link("DefaultApi", new { id = command.Id });
            response.Headers.Location = new Uri(uri);

            return response;
        }

        public void Put(Guid id, JObject json)
        {
            var name = json.Value<string>("name");
            var unitPrice = json.Value<decimal>("unitPrice");
            var entityVersion = json.Value<int>("entityVersion");
            var command = new UpdateProductCommand(id, name, unitPrice);

            // todo: refactor
            var commandHandler = new UpdateProductCommandHandler();
            var events = commandHandler.Handle(command).ToArray();

            _eventStore.SaveEvents(command.Id, entityVersion, events);
            _eventPublisher.PublishEvents(events);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}