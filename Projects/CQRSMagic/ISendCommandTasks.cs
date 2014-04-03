using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.CQRS
{
    public interface ISendCommandTasks
    {
        Task<IEnumerable<IEvent>> SendCommand { get; set; }
    }
}