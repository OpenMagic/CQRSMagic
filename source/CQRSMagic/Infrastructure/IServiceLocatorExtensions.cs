using System;
using Microsoft.Practices.ServiceLocation;

namespace CQRSMagic.Infrastructure
{
    // ReSharper disable once InconsistentNaming
    internal static class IServiceLocatorExtensions
    {
        internal static bool CanGetInstance(this IServiceLocator serviceLocator, Type service)
        {
            throw new NotImplementedException();
        }

        internal static bool TryGetInstance(this IServiceLocator serviceLocator, Type instanceType, out object instance)
        {
            throw new NotImplementedException();
        }
    }
}