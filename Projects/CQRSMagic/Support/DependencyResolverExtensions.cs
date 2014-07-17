namespace CQRSMagic.Support
{
    public static class DependencyResolverExtensions
    {
        public static TService GetService<TService>(this IDependencyResolver dependencyResolver)
        {
            return (TService) dependencyResolver.GetService(typeof(TService));
        }
    }
}
