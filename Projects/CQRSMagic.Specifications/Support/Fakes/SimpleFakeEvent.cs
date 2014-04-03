using System;

namespace Library.CQRS.Specifications.Support.Fakes
{
    public class SimpleFakeEvent : Event
    {
        public SimpleFakeEvent()
        {
            // XmlClone() requires empty constructor.
        }

        internal SimpleFakeEvent(int instance)
        {
            StringValue = string.Format("fake string {0:N0}", instance);
            IntValue = instance;
            DateValue = (new DateTime(2014, 1, 1)).AddDays(instance);
        }

        public string StringValue { get; set; }
        public int IntValue { get; set; }
        public DateTime DateValue { get; set; }
    }
}
