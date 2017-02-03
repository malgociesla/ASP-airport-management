[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AirplaneASP.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AirplaneASP.App_Start.NinjectWebCommon), "Stop")]

namespace AirplaneASP.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    using AirportService;
    using Utils;

    using AutoMapper;
    using Models.Companies;
    using AirportService.DTO;
    using Mapping;

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

                RegisterServices(kernel);
                RegisterMappings(kernel);
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
            kernel.Bind<ICityService>().To<CityService>();
            kernel.Bind<ICountryService>().To<CountryService>();
            kernel.Bind<ICompanyService>().To<CompanyService>();
            kernel.Bind<IScheduleService>().To<ScheduleService>();
            kernel.Bind<IScheduleUtils>().To<ScheduleExcelUtils>();
            kernel.Bind<IFlightStateService>().To<FlightStateService>();
            kernel.Bind<IFlightService>().To<FlightService>();
            kernel.Bind<IScheduleParser>().To<ScheduleParser>();
        }

        private static void RegisterMappings(IKernel kernel)
        {
            var config = MappingConfig.ConfigureMappings();
            var mapper = config.CreateMapper();
            kernel.Bind<IMapper>().ToConstant(mapper);
            kernel.Bind<IMapper<CompanyDTO, CompanyModel>>().To<AutoMapper<CompanyDTO,CompanyModel>>();
            kernel.Bind<IMapper<CompanyModel, CompanyDTO>> ().To<AutoMapper<CompanyModel, CompanyDTO>>();
        }
    }
}
