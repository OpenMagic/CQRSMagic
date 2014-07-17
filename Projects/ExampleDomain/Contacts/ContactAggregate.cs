using System.Threading.Tasks;
using CQRSMagic.Domain;
using CQRSMagic.Event;
using ExampleDomain.Contacts.Events;

namespace ExampleDomain.Contacts
{
    public class ContactAggregate : AggregateBase,
        IApplyEvent<CreatedContact>
    {
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }

        public Task ApplyEventAsync(CreatedContact @event)
        {
            Id = @event.AggregateId;
            Name = @event.Name;
            EmailAddress = @event.EmailAddress;

            return Task.FromResult(true);
        }
    }
}