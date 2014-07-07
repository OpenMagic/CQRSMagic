using System;
using CQRSMagic.Events.Messaging;

namespace CQRSMagic.Specifications.UnitTests.Azure.Support
{
    public class EventWithAllAzureSupportedTypes : IEvent
    {
        public EventWithAllAzureSupportedTypes()
            : this(initializeProperties: false)
        {
        }

        public EventWithAllAzureSupportedTypes(bool initializeProperties)
        {
            if (!initializeProperties)
            {
                return;
            }

            AggregateType = typeof(Exception);
            AggregateId = Guid.NewGuid();
            EventCreated = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            Bytes = new byte[1000];
            Boolean = true;
            DateTime = EventCreated.DateTime;
            Double = 1.23;
            Guid = Guid.NewGuid();
            Int = 2;
            Long = 3;
            String = "a string";
        }

        public Type AggregateType { get; private set; }
        public Guid AggregateId { get; private set; }
        public DateTimeOffset EventCreated { get; private set; }

        public byte[] Bytes { get; private set; }
        public bool Boolean { get; private set; }
        public DateTime DateTime { get; private set; }
        public double Double { get; private set; }
        public Guid Guid { get; private set; }
        public int Int { get; private set; }
        public long Long { get; private set; }
        public string String { get; private set; }
    }
}