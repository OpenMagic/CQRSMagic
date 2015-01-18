﻿using System.Web.Http;
using CQRSMagic.WebApiExample.Infrastructure;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace CQRSMagic.WebApiExample
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder
                .UseNinjectMiddleware(() => IoC.Kernel)
                .UseNinjectWebApi(config);
        }
    }
}