using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CQRSMagic
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; protected set; }

        public void ApplyEvents(IEnumerable<IEvent> events)
        {
            foreach (var e in events)
            {
                var applyEventMethod = GetApplyEventFor(e);

                applyEventMethod.Invoke(this, new object[] { e });
            }
        }

        private MethodInfo GetApplyEventFor(IEvent e)
        {
            // todo: may want to cache this
            var method = GetType().GetRuntimeMethods().SingleOrDefault(m => IsApplyEventMethod(m, e));

            if (method == null)
            {
                throw new ApplyEventException(this, e);
            }

            return method;
        }

        private static bool IsApplyEventMethod(MethodBase method, IEvent e)
        {
            if (method.Name != "ApplyEvent")
            {
                return false;
            }

            var parameters = method.GetParameters();

            return parameters.Length == 1 && parameters[0].ParameterType == e.GetType();
        }
    }
}