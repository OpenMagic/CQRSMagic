using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSMagic.Specifications.Support.Fakes
{
    public class FakeAggregateCommandHandlers : 
        AggregateCommandHandlers<FakeAggregate>, 
        IHandleCommand<FakeCommand>
    {
        public Task<IEnumerable<IEvent>> HandleCommand(FakeCommand command, IEventStore eventStore)
        {
            return Task.Factory.StartNew(() => (new IEvent[] {new SimpleFakeEvent()}).AsEnumerable());
        }
    }
}