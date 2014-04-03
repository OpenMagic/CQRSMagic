namespace CQRSMagic
{
    public interface IMessageBus
    {
        ISendCommandTasks SendCommand(ICommand command);
    }
}