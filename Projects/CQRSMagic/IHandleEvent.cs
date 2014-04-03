namespace Library.CQRS
{
    public interface IHandleEvent<in TEvent>
    {
        void HandleEvent(TEvent e);
    }
}