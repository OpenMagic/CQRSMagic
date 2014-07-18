namespace CQRSMagic.IoC
{
    public static class DependencyResolverExtensions
    {
        public static TService Get<TService>(this IDependencyResolver dependencyResolver)
        {
            return (TService) dependencyResolver.Get(typeof(TService));
        }
    }
}