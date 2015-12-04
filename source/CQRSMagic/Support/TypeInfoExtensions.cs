using System.Reflection;

namespace CQRSMagic.Support
{
    internal static class TypeInfoExtensions
    {
        internal static bool IsConcreteType(this TypeInfo typeInfo)
        {
            return (!typeInfo.IsAbstract || !typeInfo.IsInterface);
        }
    }
}
