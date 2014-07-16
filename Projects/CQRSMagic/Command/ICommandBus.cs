using System.Threading.Tasks;

namespace CQRSMagic.Command
{
    public interface ICommandBus
    {
        Task SendCommandAsync(ICommand command);
    }
}