namespace CQRSMagic
{
    public interface IHandleEvent<in TEvent>
    {
        void HandleEvent(TEvent e);
    }
}