using Autofac;
using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;
using MandaditosExpress.Models.Extensions;
using MandaditosExpress.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MandaditosExpress
{
    public class AutoFacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MainMappingProfile>();
                cfg.AddProfile<APIMappingProfile>();

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
        CreateMap<Servicio, ServicioViewModel>().ReverseMap();
        CreateMap<TipoDePago, TipoDePagoViewModel>();
        CreateMap<Envio, EnvioViewModel>().ForMember(x => x.TipoDeServicioDescripcion, x => x.MapFrom(y => y.TipoDeServicio.DescripcionTipoDeServicio))
                                          .ForMember(x => x.ClienteNombres, x => x.MapFrom(y => y.Cliente.NombreCompleto))
                                          .ForMember(x => x.ClienteFoto, x => x.MapFrom(y => y.Cliente.Foto))
                                          .ForMember(x => x.CotizacionDescripcion, x => x.MapFrom(y => y.Cotizacion.DescripcionDeCotizacion)); ;
        CreateMap<Motorizado, MotorizadoViewModel>().
        ForMember(x => x.EstadoMotorizadoClass, x => x.MapFrom(y => y.EstadoDelMotorizado == (short)EstadoDeMotorizadoEnum.Activo
          ? "badge badge-success" : y.EstadoDelMotorizado == (short)EstadoDeMotorizadoEnum.Inactivo ? "badge badge-warning" : "badge badge-primary"));
        CreateMap<Lugar, LugarViewModel>().ReverseMap();
    }
}

public class APIMappingProfile : Profile
{
    public APIMappingProfile()
    {
        CreateMap<Credito, ResponseWsCredito>().ForMember(x => x.ClienteNombres, x => x.MapFrom(y => y.Cliente.NombreCompleto))
            .ForMember(x => x.FechaDeInicio, x => x.MapFrom(y => y.FechaDeInicio.ToString("dd/MM/yyyy")))
         .ForMember(x => x.FechaDeVencimiento, x => x.MapFrom(y => y.FechaDeVencimiento.ToString("dd/MM/yyyy")))
         .ForMember(x => x.FechaDeCancelacion, x => x.MapFrom(y => y.FechaDeCancelacion.ToString("dd/MM/yyyy")))
         .ForMember(x => x.Descripcion, x => x.MapFrom(y=> "Credito principal para christopher 2022-2"))
          .ForMember(x => x.CodigoDelCredito, x => x.MapFrom(y => "CRED20211"));
    }
}