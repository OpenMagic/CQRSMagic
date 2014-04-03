using System;
using CQRSMagic.Specifications.Support.Fakes;

namespace CQRSMagic.Specifications.Support
{
    internal static class ExtensionMethods
    {
        internal static Type GetTypeFromName(this string typeName)
        {
            var type = Type.GetType(string.Format("{0}.{1}", GetFakesNamespace(), typeName));

            if (type == null)
            {
               throw new Exception(string.Format("Could not find {0} type.", typeName));
            }

            return type;
        }

        private static string GetFakesNamespace()
        {
            var anyFakeType = typeof(FakeEventStoreRepository);
            return anyFakeType.Namespace;
        }
    }
}