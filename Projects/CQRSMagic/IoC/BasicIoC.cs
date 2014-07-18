using System;
using System.Collections.Generic;
using NullGuard;

namespace CQRSMagic.IoC
{
    public class BasicIoC : IDependencyResolver
    {
        private readonly Dictionary<Type, Func<BasicIoC, object>> Bindings;
        private readonly Func<Type, object> UnboundTypeFactory;

        public BasicIoC([AllowNull] Func<Type, object> unboundTypeFactory)
        {
            if (unboundTypeFactory == null)
            {
                unboundTypeFactory = GetUnboundType;
            }

            UnboundTypeFactory = unboundTypeFactory;
            Bindings = new Dictionary<Type, Func<BasicIoC, object>>();
        }

        public object Get(Type type)
        {
            Func<BasicIoC, object> valueFactory;

            if (Bindings.TryGetValue(type, out valueFactory))
            {
                return valueFactory(this);
            }

            try
            {
                return UnboundTypeFactory(type);
            }
            catch (Exception innerException)
            {
                throw new InvalidOperationException(string.Format("{0} requires a binding.", type), innerException);
            }
        }

        private static object GetUnboundType(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public T Get<T>()
        {
            return (T) Get(typeof(T));
        }

        public void Bind<T>(T toValue)
        {
            Bind(typeof(T), container => toValue);
        }

        public void Bind<T>(Func<BasicIoC, T> toValueFactory)
        {
            Bind(typeof(T), container => toValueFactory(container));
        }

        public void Bind(Type type, Func<BasicIoC, object> toValueFactory)
        {
            Bindings.Add(type, toValueFactory);
        }
    }
}