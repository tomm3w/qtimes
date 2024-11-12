using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Modules;

namespace SeatQ.core.api.Infrastructure
{
	public class NinjectHttpResolver : IDependencyResolver
	{
		public IKernel Kernel { get; private set; }
		public NinjectHttpResolver(params INinjectModule[] modules)
		{
			Kernel = new StandardKernel(modules);
		}

		public object GetService(Type serviceType)
		{
			return Kernel.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return Kernel.GetAll(serviceType);
		}

		public void Dispose()
		{
			//Do Nothing
		}

		public IDependencyScope BeginScope()
		{
			return this;
		}
	}
}