using System;
using System.Reflection;

namespace CQRSMagic.Support
{
    internal static class TypeExtensions
    {
        internal static bool Implements(this Type type, Type implements)
        {
            var typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsGenericType)
            {
                return false;
            }

            return typeInfo.GetGenericTypeDefinition() == implements;
        }
    }
}
