using System;

namespace CQRSMagic
{
    public class EntityNotFoundException<T> : Exception
    {
        public EntityNotFoundException(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}