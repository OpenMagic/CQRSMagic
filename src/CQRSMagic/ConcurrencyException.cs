using System;

namespace CQRSMagic
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(Guid id, int expectedVersion, int actualVersion)
            : base(string.Format("Cannot update entity '{0}' because expected version to be '{1}' but it is '{2}'.", id, expectedVersion, actualVersion))
        {
        }
    }
}