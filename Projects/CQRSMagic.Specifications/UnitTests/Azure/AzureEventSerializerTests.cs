using System;
using AzureMagic;
using CQRSMagic.Azure;
using CQRSMagic.Specifications.Support;
using CQRSMagic.Specifications.UnitTests.Azure.Support;
using FluentAssertions;
using Xunit;

namespace CQRSMagic.Specifications.UnitTests.Azure
{
    public class AzureEventSerializerTests
    {
        protected static readonly IAzureEventSerializer Serializer = new AzureEventSerializer();

        public class Deserialize
        {
            [Fact]
            public void ShouldReturn_Event()
            {
                // Given
                var beforeSerializationEvent = new EventWithAllAzureSupportedTypes(initializeProperties: true);
                var entity = Serializer.Serialize(beforeSerializationEvent);
                
                // When
                var afterSerializationEvent = Serializer.Deserialize(entity);

                // Then
                afterSerializationEvent.ShouldBeEquivalentTo(beforeSerializationEvent);
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhen_entity_IsNull()
            {
                ActionShould.Throw<ArgumentNullException>(() => Serializer.Deserialize(null));
            }
        }

        public class Serialize
        {
            [Fact]
            public void ShouldReturn_DynamicTableEntity_WithPopulated_Properties()
            {
                // Given
                var @event = new EventWithAllAzureSupportedTypes(initializeProperties: true);

                // When
                var entity = Serializer.Serialize(@event);

                // Then
                entity.PartitionKey.Should().Be(@event.AggregateId.ToPartitionKey());
                entity.RowKey.Should().Be(@event.EventCreated.ToRowKey());
                entity.Properties.Keys.ShouldBeEquivalentTo(new[]
                {
                    "AggregateType",
                    "EventType",
                    "Bytes",
                    "Boolean",
                    "DateTime",
                    "Double",
                    "Guid",
                    "Int",
                    "Long",
                    "String"
                });

                entity.Properties["AggregateType"].StringValue.Should().Be(@event.AggregateType.AssemblyQualifiedName);
                entity.Properties["EventType"].StringValue.Should().Be(@event.GetType().AssemblyQualifiedName);
                entity.Properties["Bytes"].BinaryValue.Should().BeEquivalentTo(@event.Bytes);
                entity.Properties["Boolean"].BooleanValue.Should().Be(@event.Boolean);
                entity.Properties["DateTime"].DateTime.Should().Be(@event.DateTime);
                entity.Properties["Double"].DoubleValue.Should().Be(@event.Double);
                entity.Properties["Guid"].GuidValue.Should().Be(@event.Guid);
                entity.Properties["Int"].Int32Value.Should().Be(@event.Int);
                entity.Properties["Long"].Int64Value.Should().Be(@event.Long);
                entity.Properties["String"].StringValue.Should().Be(@event.String);
            }

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhen_event_IsNull()
            {
                ActionShould.Throw<ArgumentNullException>(() => Serializer.Serialize(null));
            }
        }
    }
}
