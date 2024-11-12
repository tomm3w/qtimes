using Ninject;
//using iDestn.core.dal.Infrastructure.Repositories;
using Ninject.Modules;
using SeatQ.core.dal.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using common.api.Queries;
using common.api.Commands;
using common.dal;
using common.api;
using SeatQ.core.api.Queries.GetReadyMessages;
using SeatQ.core.dal.Repositories;
using SeatQ.core.api.Infrastructure.NexmoMessaging;
using SeatQ.core.api.Services;

namespace SeatQ.core.api.Infrastructure
{
    /// <summary>
    /// Http module implementation
    /// </summary>
    public class NinjectHttpModules
    {
        /// <summary>
        /// Return Lists of Modules in the Application
        /// </summary>
        public static NinjectModule[] Modules
        {
            get
            {
                return new NinjectModule[] { new MainModule() };
            }
        }

        /// <summary>
        /// Main Module For Application
        /// </summary>
        public class MainModule : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind<IFactory>().To<NinjectFactory>();
                Kernel.Bind<IApplicationFacade>().To<ApplicationFacade>();
                Kernel.Bind<IAccountInfoRepository>().To<AccountInfoRepository>();
                Kernel.Bind<IMessageRepository>().To<MessageRepository>();
                Kernel.Bind<IWaitListRepository>().To<WaitListRepository>();
                Kernel.Bind<IMessaging>().To<Messaging>();
                Kernel.Bind<IPasstrekClient>().To<PasstrekClient>();

                Kernel.Bind(typeof(userinfo.dal.Repositories.IUserRepository)).To(typeof(userinfo.dal.Repositories.UserRepository));
                //TODO: Bind to Concrete Types Here
                foreach (var coreAssembly in GetCoreAssemblies())
                {
                    var types = coreAssembly.GetTypes();
                    RegisterCoreType(Kernel, types);
                }
            }

            private static void RegisterCoreType(IKernel kernel, IEnumerable<Type> types)
            {
                foreach (var type in types)
                {
                    var searchedQuery = type.GetInterfaces().SingleOrDefault(x => x.IsGenericType &&
                        x.GetGenericTypeDefinition() == typeof(IQuery<,>));

                    var searchedCommand = type.GetInterfaces().SingleOrDefault(x => x.IsGenericType &&
                        (x.GetGenericTypeDefinition() == typeof(ICommand<>) || x.GetGenericTypeDefinition() == typeof(IAsyncCommand<,>)));

                    var searchedRepositories = type.GetInterfaces().SingleOrDefault(x => x.IsGenericType &&
                        x.GetGenericTypeDefinition() == typeof(IGenericRepository<,>));

                    if (null != searchedQuery)
                        kernel.Bind(searchedQuery).To(type);

                    if (null != searchedCommand)
                        kernel.Bind(searchedCommand).To(type);

                    if (null != searchedRepositories)
                        kernel.Bind(searchedRepositories).To(type);
                }
            }
            private static IEnumerable<Assembly> GetCoreAssemblies()
            {
                yield return Assembly.GetAssembly(typeof(GetReadyMessagesQuery));
                yield return Assembly.GetAssembly(typeof(ReadyMessageRepo));
            }

        }
    }
}