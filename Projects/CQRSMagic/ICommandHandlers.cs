using Library.CQRS.Support;

namespace Library.CQRS
{
    public interface ICommandHandlers
    {
        ICommandHandler GetCommandHandler(ICommand command);
    }
}