using Autofac;
using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;
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
        CreateMap<Servicio, ServicioViewModel>().ReverseMap();
        CreateMap<TipoDePago, TipoDePagoViewModel>();

        CreateMap<Envio, EnvioViewModel>().ForMember(x => x.TipoDeServicioDescripcion, x => x.MapFrom(y => y.TipoDeServicio.DescripcionTipoDeServicio))
                                          .ForMember(x => x.ClienteNombres, x => x.MapFrom(y => y.Cliente.NombreCompleto))
                                          .ForMember(x => x.ClienteFoto, x => x.MapFrom(y => y.Cliente.Foto))
                                          .ForMember(x => x.CotizacionDescripcion, x => x.MapFrom(y => y.Cotizacion.DescripcionDeCotizacion))
                                          .ForMember(x => x.EstadoDelEnvioClass, x => x.MapFrom(y => y.EstadoDelEnvio ==(short)EstadoDelEnvioEnum.Solicitud ? "badge badge-primary" : y.EstadoDelEnvio== (short)EstadoDelEnvioEnum.EnProceso ? "badge badge-success" : "badge badge-warning"))
                                          .ForMember(x => x.EstadoDelEnvioDescripcion, x => x.MapFrom(y => y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Solicitud ? EstadoDelEnvioEnum.Solicitud.ToString() : y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso ? EstadoDelEnvioEnum.EnProceso.ToString() : EstadoDelEnvioEnum.Realizado.ToString()))
                                          .ForMember(x => x.MotorizadoNombres, x => x.MapFrom(y => y.Motorizado.NombreCompleto));

        CreateMap<Motorizado, MotorizadoViewModel>().
                                    ForMember(x=> x.EstadoMotorizadoClass, x=> x.MapFrom(y=> y.EstadoDelMotorizado== (short) EstadoDeMotorizadoEnum.Activo 
                                    ? "badge badge-primary" : y.EstadoDelMotorizado== (short) EstadoDeMotorizadoEnum.Inactivo ? "badge badge-warning" : "badge badge-success" )).ReverseMap();


        CreateMap<Motorizado, AsignacionMotorizadoViewModel>().
                                    ForMember(x => x.EstadoMotorizadoClass, x => x.MapFrom(y => y.EstadoDelMotorizado == (short)EstadoDeMotorizadoEnum.Activo
                                      ? "badge badge-primary" : y.EstadoDelMotorizado == (short)EstadoDeMotorizadoEnum.Inactivo ? "badge badge-warning" : "badge badge-success"))
                                    .ForMember(x => x.EstadoMotorizadoDescripcion, x => x.MapFrom(y => y.EstadoDelMotorizado == (short)EstadoDeMotorizadoEnum.Inactivo ? EstadoDeMotorizadoEnum.Inactivo.ToString() : y.EstadoDelMotorizado== (short)EstadoDeMotorizadoEnum.Activo ? EstadoDeMotorizadoEnum.Activo.ToString() : EstadoDeMotorizadoEnum.Ocupado.ToString()))
                                    .ForMember(x=> x.Nombres, x=> x.MapFrom(y=> y.PrimerNombre + " " + y.PrimerApellido + " " + y.SegundoApellido)).ReverseMap();


        CreateMap<Lugar, LugarViewModel>().ReverseMap();
        CreateMap<Cliente, ClientePagoViewModel>()
            .ForMember(x => x.Nombres, x => x.MapFrom(y => y.PrimerNombre + " " + y.PrimerApellido + " " + y.SegundoApellido)).ReverseMap();
        CreateMap<Envio, EnvioPagoViewModel>().ForMember(x=> x.DistanciaEntregaRecep, x=> x.MapFrom(y=> y.DistanciaEntregaRecep.ToString("0.##")))
            .ReverseMap();

        CreateMap<Moneda, MonedaViewModel>().ForMember(x => x.NombreDeMoneda, x => x.MapFrom(y => y.NombreDeMoneda + " " + y.Abreviatura)).ReverseMap();
        
        //el envio se suma al credito pero si es un envio en proceso o finalizado.
        CreateMap<Credito, CreditoViewModel>().ForMember(x=> x.NombreCompletoCliente, x=> x.MapFrom(y=> y.Cliente.PrimerNombre + " " + y.Cliente.PrimerApellido + " " + y.Cliente.SegundoApellido))
            .ForMember(x=> x.MontoDelCredito, x=> x.MapFrom(y=> y.Envios.Where(w=> w.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso || w.EstadoDelEnvio == (short) EstadoDelEnvioEnum.Realizado)
            .Sum(z=> z.MontoTotalDelEnvio))).ReverseMap();

        CreateMap<Pago, PagoViewModel>().ReverseMap();
        CreateMap<Pago, IndexPagoViewModel>().ForMember(x=> x.MonedaDescripcion, x=> x.MapFrom(y=> y.Moneda.NombreDeMoneda + " " + y.Moneda.Abreviatura))
            .ForMember(x=> x.TipoDePagoDescripcion, x=> x.MapFrom(y=> y.TipoDePago.Descripcion))
            .ForMember(x => x.EnvioCodigo, x => x.MapFrom(y => y.Envio!= null ? y.Envio.CodigoDeEnvio : "-"))
            .ForMember(x => x.CreditoCodigo, x => x.MapFrom(y => y.Credito!=null ? y.Credito.CodigoDelCredito : "-"))
            .ForMember(x => x.ConceptoDelPago, x => x.MapFrom(y => y.Envio!= null ? "Envio" : "Crédito"))
            .ForMember(x => x.ConceptoDelPagoClass, x => x.MapFrom(y => y.Envio != null ? "badge badge-primary" : "badge badge-warning"))
            .ForMember(x=> x.ClienteNombres, x=> x.MapFrom(y=> y.Envio!=null ? y.Envio.Cliente.PrimerNombre + " " + y.Envio.Cliente.PrimerApellido + " " + y.Envio.Cliente.SegundoApellido :
            y.Credito.Cliente.PrimerNombre + " " + y.Credito.Cliente.PrimerApellido + " " + y.Credito.Cliente.SegundoApellido)).ReverseMap();

        CreateMap<Envio, IndexEnvioViewModel>().
                            ForMember(x => x.EstadoDelEnvioClass, x => x.MapFrom(y => y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Solicitud? "badge badge-success" : y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso ? "badge badge-primary" : y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Rechazado ? "badge badge-info" : "badge badge-danger"))
                            .ForMember(x => x.EstadoDelEnvioText, x => x.MapFrom(y => y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Solicitud ? EstadoDelEnvioEnum.Solicitud.ToString() : y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso ? EstadoDelEnvioEnum.EnProceso.ToString() 
                            : y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado ? EstadoDelEnvioEnum.Realizado.ToString() : EstadoDelEnvioEnum.Rechazado.ToString()))
                            .ForMember(x => x.Cliente, x => x.MapFrom(y => y.Cliente.PrimerNombre + " " + y.Cliente.PrimerApellido + " " + y.Cliente.SegundoApellido))
                            .ForMember(x => x.Finalizado, x => x.MapFrom(y => y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado ? true : false ))
                            .ForMember(x => x.Rechazado, x => x.MapFrom(y => y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Rechazado ? true : false))
                            .ForMember(x => x.EstaRetrasado, x=> x.MapFrom(y=> y.EsUrgente && y.EstadoDelEnvio==(short)EstadoDelEnvioEnum.EnProceso && y.FechaDelEnvio.AddMinutes(35) > DateTime.Now ? true : !y.EsUrgente && y.EstadoDelEnvio==(short)EstadoDelEnvioEnum.EnProceso && y.FechaDelEnvio.AddMinutes(90) > DateTime.Now ? true : false)).ReverseMap();
        CreateMap<Envio, EnvioHistorialViewModel>().ReverseMap();
        CreateMap<Envio, EnviosCreditoViewModel>().ReverseMap();
        CreateMap<Persona, UsuarioViewModel>().ForMember(it=> it.Nombres, it=> it.MapFrom(y=> y.PrimerNombre + " " + y.PrimerApellido + " " + y.SegundoApellido)).ReverseMap();
        CreateMap<Cliente, ClienteBusquedasViewModel>().ForMember(it=> it.NombreCompleto, it=> it.MapFrom(y=> y.PrimerNombre + " " + y.PrimerApellido + " " + y.SegundoApellido)).ReverseMap();

    }
}