using System;

namespace CQRSMagic
{
    public class EntityNotFoundException<TEntity> : Exception where TEntity : IEntity
    {
        public EntityNotFoundException(Guid id)
            : base(CreateMessage(typeof(TEntity), id))
        {
        }

        private static string CreateMessage(Type entityType, Guid id)
        {
            return string.Format("Cannot find {0}/{1}.", entityType.Name, id);
        }
    }
}