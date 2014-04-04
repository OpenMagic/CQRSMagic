using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic.Specifications.Support.Fakes
{
    internal class FakeAggregate : IAggregate
    {
        public Task SendEvents(IEnumerable<IEvent> events)
        {
            throw new System.NotImplementedException();
        }
    }
}