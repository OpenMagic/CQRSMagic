using Microsoft.Practices.ServiceLocation;

namespace CQRSMagic.Support
{
    public static class ServiceLocatorExtensions
    {
        public static TService Get<TService>(this IServiceLocator serviceLocator)
        {
            return (TService) serviceLocator.GetService(typeof(TService));
        }
    }
}