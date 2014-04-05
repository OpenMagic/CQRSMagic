using System;
using System.Linq;
using System.Linq.Expressions;

namespace CQRSMagic.Support
{
    public static class ExtensionMethods
    {
        public static bool Implements(this Type type, Type implementType)
        {
            // todo: move to OpenMagic?

            if (type.IsGenericType || type.IsAbstract)
            {
                return false;
            }
            return type.GetInterfaces().Any(@interface => @interface.Is(implementType));
        }

        private static bool Is(this Type type, Type isType)
        {
            // todo: move to OpenMagic?

            return type.IsGenericType && type.GetGenericTypeDefinition() == isType;
        }
    }
}