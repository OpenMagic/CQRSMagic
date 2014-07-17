using System;

namespace CQRSMagic.Event
{
    public class EventException : Exception
    {
        public EventException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}