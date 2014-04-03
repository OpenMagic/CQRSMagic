namespace Library.CQRS
{
    public interface IMessageBus
    {
        ISendCommandTasks SendCommand(ICommand command);
    }
}