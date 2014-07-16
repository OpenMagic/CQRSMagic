using System.Threading.Tasks;

namespace CQRSMagic.Command
{
    public class CommandBus : ICommandBus
    {
        public Task SendCommandAsync(ICommand command)
        {
            return Task.FromResult(false);
        }
    }
}