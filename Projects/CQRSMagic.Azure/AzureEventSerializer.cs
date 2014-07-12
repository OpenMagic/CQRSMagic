using System;
using System.Linq;
using System.Reflection;
using Anotar.CommonLogging;
using CQRSMagic.Azure.Support;
using CQRSMagic.Events.Messaging;
using Microsoft.WindowsAzure.Storage.Table;

namespace CQRSMagic.Azure
{
    public class AzureEventSerializer : IAzureEventSerializer
    {
        public IEvent Deserialize(DynamicTableEntity entity)
        {
            // todo: caching?
            // todo: compiled lambda?

            var eventType = Type.GetType(entity["EventType"].StringValue);
            var eventProperties = eventType.GetProperties();
            var @event = (IEvent) Activator.CreateInstance(eventType);

            var query =
                from entityProperty in entity.Properties
                let propertyInfo = eventProperties.SingleOrDefault(p => p.Name == entityProperty.Key)
                where propertyInfo != null
                select new {entityProperty, propertyInfo};

            eventProperties.Single(p => p.Name == "AggregateId").SetValue(@event, Guid.Parse(entity.PartitionKey));
            eventProperties.Single(p => p.Name == "EventCreated").SetValue(@event, DateTimeOffset.Parse(entity.RowKey));

            foreach (var item in query)
            {
                LogTo.Debug("Updating {0}.{1} from {2}.", eventType.Name, item.propertyInfo.Name, item.entityProperty.Key);

                var entityValue = GetValueFromEntityProperty(item.entityProperty.Value);

                if (item.propertyInfo.PropertyType.Name == "Type")
                {
                    entityValue = Type.GetType(entityValue.ToString());
                }

                item.propertyInfo.SetValue(@event, entityValue);
            }

            return @event;
        }

        public DynamicTableEntity Serialize(IEvent @event)
        {
            // todo: caching?
            // todo: compiled lambda?

            var eventProperties = @event.GetType().GetProperties().Where(p => p.Name != "AggregateId" && p.Name != "EventCreated");
            var entityProperties = eventProperties.Select(p => new {p.Name, Property = CreateEntityProperty(@event, p)}).ToList();

            entityProperties.Insert(1, new {Name = "EventType", Property = new EntityProperty(@event.GetType().AssemblyQualifiedName)});

            var entity = new DynamicTableEntity
            {
                PartitionKey = @event.AggregateId.ToPartitionKey(),
                RowKey = @event.EventCreated.ToRowKey(),
                Properties = entityProperties.ToDictionary(p => p.Name, p => p.Property)
            };

            return entity;
        }

        private object GetValueFromEntityProperty(EntityProperty entityProperty)
        {
            switch (entityProperty.PropertyType)
            {
                case EdmType.String:
                    return entityProperty.StringValue;

                case EdmType.Binary:
                    return entityProperty.BinaryValue;

                case EdmType.Boolean:
                    return entityProperty.BooleanValue;

                case EdmType.DateTime:
                    return entityProperty.DateTime;

                case EdmType.Double:
                    return entityProperty.DoubleValue;

                case EdmType.Guid:
                    return entityProperty.GuidValue;

                case EdmType.Int32:
                    return entityProperty.Int32Value;

                case EdmType.Int64:
                    return entityProperty.Int64Value;

                default:
                    throw new NotSupportedException(string.Format("{0} is not supported.", entityProperty.PropertyType));
            }
        }

        private static EntityProperty CreateEntityProperty(IEvent @event, PropertyInfo propertyInfo)
        {
            try
            {
                var value = propertyInfo.GetValue(@event, null);

                if (propertyInfo.PropertyType == typeof(Type))
                {
                    value = ((Type) value).AssemblyQualifiedName;
                }

                var property = EntityProperty.CreateEntityPropertyFromObject(value);

                return property;
            }
            catch (Exception ex)
            {
                var message = string.Format("Cannot create entity property for {0}.{1}.", @event.GetType(), propertyInfo.Name);
                var exception = new Exception(message, ex);

                throw exception;
            }
        }
    }
}