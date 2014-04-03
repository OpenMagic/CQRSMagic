using System;

namespace CQRSMagic.Specifications.Support
{
    internal static class ExtensionMethods
    {
        internal static Type GetTypeFromName(this string aggregateType)
        {
            return Type.GetType("Library.CQRS.Specifications.Support.Fakes." + aggregateType);
        }
    }
}