using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSMagic
{
    public interface ISendCommandTasks
    {
        Task<IEnumerable<IEvent>> SendCommand { get; set; }
    }
}