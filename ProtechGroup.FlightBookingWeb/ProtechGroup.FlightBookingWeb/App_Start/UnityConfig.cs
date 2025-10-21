using ProtechGroup.Application.Interfaces;
using ProtechGroup.Application.Services;
using ProtechGroup.Domain.Interfaces;
using ProtechGroup.Infrastructure.Repositories;
using System;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;
using AutoMapper;
using ProtechGroup.Infrastructure.Mapping;
using ProtechGroup.Infrastructure.FlightProviders;
using ProtechGroup.Application.Common;

namespace ProtechGroup.FlightBookingWeb
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer Container => container.Value;

        public static void RegisterComponents()
        {
            var container = Container;

            // AutoMapper config
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();
            container.RegisterInstance(mapper);

            // Đăng ký Repository
            container.RegisterType<ISearchInputRepository, SearchInputRepository>();
            container.RegisterType<IAirportRepository, AirportRepository>();
            container.RegisterType<ISearchWSHistoryRepository, SearchWSHistoryRepository>();
            container.RegisterType<IServiceFeeRepository, ServiceFeeRepository>();
            container.RegisterType<INewRepository, NewRepository>();
            container.RegisterType<IHotelRepository, HotelRepository>();
            

            // Đăng ký Service
            container.RegisterType<ISearchInputService, SearchInputService>();
            container.RegisterType<IAirportService, AirportService>();
            container.RegisterType<ISearchWSHistoryService, SearchWSHistoryService>();
            container.RegisterType<IServiceFeeService, ServiceFeeService>();
            container.RegisterType<INewsService, NewsService>();
            container.RegisterType<IHotelService, HotelService>();




            container.RegisterType<IMethodService, MethodService>();
            container.RegisterType<IBamBooAirWaysMethod, BamBooAirWaysMethod>();
            container.RegisterType<IBambooAirwaysService, BambooAirwaysService>();
            container.RegisterType<IBambooAirwaysProvider, BambooAirwaysProvider>();
            container.RegisterType<IVietJetsMethod, VietJetsMethod>();
            container.RegisterType<IVietJetsService, VietJetsService>();
            container.RegisterType<IVietJetsProvider, VietJetsProvider>();
            container.RegisterType<IVietNamAirLinesProvider, VietNamAirLinesProvider>();
            container.RegisterType<IVietNamAirLinesMethod, VietNamAirLinesMethod>();
            container.RegisterType<IVietNamAirLinesService, VietNamAirLinesService>();

            // Kết nối Unity với MVC
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            // Có thể bổ sung đăng ký type khác tại đây nếu cần
        }
    }
}