using Autofac;
using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MandaditosExpress
{
    public class AutoFacModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MainMappingProfile>();

            })).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            }).As<IMapper>().InstancePerLifetimeScope();
        }
    }
}

public class MainMappingProfile : Profile
{
    public MainMappingProfile()
    {
        CreateMap<TipoDeServicio, TipoDeServicioViewModel>();
        CreateMap<Servicio, ServicioViewModel>();
        CreateMap<TipoDePago, TipoDePagoViewModel>();
    }
}