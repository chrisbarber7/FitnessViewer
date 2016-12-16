[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(FitnessViewer.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(FitnessViewer.App_Start.NinjectWebCommon), "Stop")]

namespace FitnessViewer.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Infrastructure.Interfaces;
    using Infrastructure.Data;
    using System.Web.Http;
    using Infrastructure.Repository;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
                kernel.Bind<IActivityDtoRepository>().To<ActivityDtoRepository>().InRequestScope();
                kernel.Bind<ICoordsDtoRepository>().To<CoordsDtoRepository>().InRequestScope();
                kernel.Bind<IGraphStreamDtoRepository>().To<GraphStreamDtoRepository>().InRequestScope();
                kernel.Bind<ITimeDistanceBySportRepository>().To<TimeDistanceBySportRepository>().InRequestScope();
                kernel.Bind<IPeriodDtoRepository>().To<PeriodDtoRepository>().InRequestScope();
                kernel.Bind<IWeightByDayDtoRepository>().To<WeightByDayDtoRepository>().InRequestScope();

                // needed for classs dervived from apicontroller.
                GlobalConfiguration.Configuration.DependencyResolver = kernel.Get<System.Web.Http.Dependencies.IDependencyResolver>();
                
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

        }        
    }
}
