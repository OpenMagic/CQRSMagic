using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.CQRS
{
    public class SendCommandTasks : ISendCommandTasks
    {
        public Task EventStore;
        public Task Subscriptions;
        public Task<IEnumerable<IEvent>> SendCommand { get; set; }
    }
}