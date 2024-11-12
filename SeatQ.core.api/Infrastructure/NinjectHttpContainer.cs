using System.Web.Http;
using Ninject;
using Ninject.Modules;

namespace SeatQ.core.api.Infrastructure
{
    public class NinjectHttpContainer
    {
        private static NinjectHttpResolver _resolver;

        //Register Ninject Modules
        public static void RegisterModules(NinjectModule[] modules, HttpConfiguration httpConfig)
        {
            _resolver = new NinjectHttpResolver(modules);
            //GlobalConfiguration.Configuration.DependencyResolver = _resolver;
            httpConfig.DependencyResolver = _resolver;
        }

        //Manually Resolve Dependencies
        public static T Resolve<T>()
        {
            return _resolver.Kernel.Get<T>();
        }
    }
}