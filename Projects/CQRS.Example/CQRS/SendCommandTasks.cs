using System.Collections.Generic;
using System.Threading.Tasks;
using CQRS.Example.CQRS.Events;

namespace CQRS.Example.CQRS
{
    public class SendCommandTasks : ISendCommandTasks
    {
        public SendCommandTasks(Task<IEnumerable<IEvent>> handleCommand, Task saveEvents, Task publishEvents)
        {
            SaveEvents = saveEvents;
            HandleCommand = handleCommand;
            PublishEvents = publishEvents;
        }

        public Task<IEnumerable<IEvent>> HandleCommand { get; private set; }

        public Task SaveEvents { get; private set; }

        public Task PublishEvents { get; private set; }

        public void WaitAll()
        {
            Task.WaitAll(new[] {HandleCommand, SaveEvents, PublishEvents});
        }
    }
}