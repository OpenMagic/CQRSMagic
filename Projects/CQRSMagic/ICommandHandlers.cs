namespace CQRSMagic
{
    public interface ICommandHandlers
    {
        ICommandHandler GetCommandHandler(ICommand command);
    }
}