using System;

namespace CQRSMagic
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(Guid id, int expectedVersion, int actualVersion)
            : base(string.Format("Cannot update entity/{0} because entity version is {1} but expected it to be {2}.", id, actualVersion, expectedVersion))
        {
        }
    }
}