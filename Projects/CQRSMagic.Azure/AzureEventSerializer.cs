using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Anotar.CommonLogging;
using AzureMagic.Tables;
using CQRSMagic.Event;
using Microsoft.WindowsAzure.Storage.Table;

namespace CQRSMagic.Azure
{
    public class AzureEventSerializer : IAzureEventSerializer
    {
        private static readonly string TransactionIndexFormat;
        private const int MaxEventsPerTransaction = 999;

        static  AzureEventSerializer()
        {
            TransactionIndexFormat = "D" + MaxEventsPerTransaction.ToString(CultureInfo.InvariantCulture).Length;
        }

        public AzureEventSerializer()
        {
            MaximumEventsPerTransaction = MaxEventsPerTransaction;
        }

        public int MaximumEventsPerTransaction { get; private set; }

        public IEvent Deserialize(DynamicTableEntity entity)
        {
            // todo: caching?
            // todo: compiled lambda?

            var eventType = Type.GetType(entity["EventType"].StringValue);
            var eventProperties = eventType.GetProperties();
            var @event = CreateEvent(eventType, entity, eventProperties);

            var query =
                from entityProperty in entity.Properties
                let propertyInfo = eventProperties.SingleOrDefault(p => p.Name == entityProperty.Key)
                where propertyInfo != null
                select new {entityProperty, propertyInfo};

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

        public DynamicTableEntity Serialize(IEvent @event, int transactionIndex)
        {
            // todo: caching?
            // todo: compiled lambda?

            var eventProperties = @event.GetType().GetProperties().Where(p => p.Name != "AggregateId" && p.Name != "EventCreated");
            var entityProperties = eventProperties.Select(p => new {p.Name, Property = CreateEntityProperty(@event, p)}).ToList();

            entityProperties.Insert(1, new {Name = "EventType", Property = new EntityProperty(@event.GetType().AssemblyQualifiedName)});

            var entity = new DynamicTableEntity
            {
                PartitionKey = @event.AggregateId.ToPartitionKey(),
                RowKey = string.Format("{0}-{1}", @event.EventCreated.ToRowKey(), transactionIndex.ToString(TransactionIndexFormat)),
                Properties = entityProperties.ToDictionary(p => p.Name, p => p.Property)
            };

            return entity;
        }

        private IEvent CreateEvent(Type eventType, DynamicTableEntity entity, PropertyInfo[] eventProperties)
        {
            const BindingFlags bindingFlags = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var @event = (IEvent) Activator.CreateInstance(eventType, bindingFlags, null, null, null);

            eventProperties.Single(p => p.Name == "AggregateId").SetValue(@event, Guid.Parse(entity.PartitionKey));
            eventProperties.Single(p => p.Name == "EventCreated").SetValue(@event, DateTimeOffset.Parse(RemoveTransactionIndexFromRowKey(entity.RowKey)));

            return @event;
        }

        private string RemoveTransactionIndexFromRowKey(string rowKey)
        {
            var index = rowKey.LastIndexOf('-');
            var value = rowKey.Substring(0, index);

            return value;
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