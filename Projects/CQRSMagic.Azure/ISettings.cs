namespace CQRSMagic.Azure
{
    public interface ISettings : CQRSMagic.ISettings
    {
        string ConnectionString { get; }
        string EventsTableName { get; }
    }
}

