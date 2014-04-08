using CQRS.Example.CQRS.Commands;

namespace CQRS.Example.CQRS
{
    public interface IMessageBus
    {
        ISendCommandTasks SendCommand(ICommand command);
    }
}