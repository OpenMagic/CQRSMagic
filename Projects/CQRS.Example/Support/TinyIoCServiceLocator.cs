using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using TinyIoC;

namespace CQRS.Example.Support
{
    public class TinyIoCServiceLocator : ServiceLocatorImplBase
    {
        private readonly TinyIoCContainer Container;

        public TinyIoCServiceLocator(TinyIoCContainer container)
        {
            Container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (key == null)
            {
                return Container.Resolve(serviceType);
            }

            return Container.Resolve(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return Container.ResolveAll(serviceType);
        }
    }
}
