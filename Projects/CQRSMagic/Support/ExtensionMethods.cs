using System;
using System.Linq;

namespace CQRSMagic.Support
{
    public static class ExtensionMethods
    {
        public static bool Implements(this Type type, Type implementType)
        {
            if (type.IsGenericType || type.IsAbstract)
            {
                return false;
            }
            return type.GetInterfaces().Any(@interface => @interface.Is(implementType));
        }

        public static bool Is(this Type type, Type isType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == isType;
        }
    }
}