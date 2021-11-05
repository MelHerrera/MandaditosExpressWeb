namespace MandaditosExpress.Migrations
{
    using MandaditosExpress.Models.Enum;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MandaditosExpress.Models.MandaditosDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MandaditosExpress.Models.MandaditosDB context)
        {
            context.Personas.AddOrUpdate(
            p => p.Id,
           new Models.Persona
           {
               PrimerNombre = "Andrew",
               SegundoNombre = "Peters",
               PrimerApellido = "Loaswer",
               SegundoApellido = "kolyter",
               CorreoElectronico = "Andrew@gmail.com",
               Telefono = "82345432",
               Sexo = "Masculino",
               Cedula = "001-110294-5000F",
               Direccion = "Laureles Sur calle del ministerio 2 C arriba",
               Foto = null,
               FechaIngreso = DateTime.Now
           },
            new Models.Persona
            {
                PrimerNombre = "Walthy",
                SegundoNombre = "Freybnui",
                PrimerApellido = "Liustgh",
                SegundoApellido = "litfer",
                CorreoElectronico = "Walthy@gmail.com",
                Telefono = "87689209",
                Sexo = "Masculino",
                Cedula = "001-110294-5000F",
                Direccion = "Laureles Sur calle del ministerio 2 C arriba",
                Foto = null,
                FechaIngreso = DateTime.Now
            },
                 new Models.Persona
                 {
                     PrimerNombre = "Huilerty",
                     SegundoNombre = "kinety",
                     PrimerApellido = "Rodriyth",
                     SegundoApellido = "meindret",
                     CorreoElectronico = "Huilerty@gmail.com",
                     Telefono = "87689209",
                     Sexo = "Masculino",
                     Cedula = "001-110294-5000F",
                     Direccion = "Laureles Sur calle del ministerio 2 C arriba",
                     Foto = null,
                     FechaIngreso = DateTime.Now
                 }
          );

            context.Clientes.AddOrUpdate(
                p => p.Id,
                       new Models.Cliente
                       {
                           PrimerNombre = "Walter",
                           SegundoNombre = "Lucindro",
                           PrimerApellido = "Valladares",
                           SegundoApellido = "Valles",
                           CorreoElectronico = "Valladares@gmail.com",
                           Telefono = "87987658",
                           Sexo = "Masculino",
                           Cedula = "003-080797-7500F",
                           Direccion = "Laureles Sur calle del ministerio 2 C arriba",
                           Foto = null,
                           FechaIngreso = DateTime.Now,
                           EsEmpresa = false
                       },
                         new Models.Cliente
                         {
                             PrimerNombre = "Freydis",
                             SegundoNombre = "Greyddon",
                             PrimerApellido = "Marciles",
                             SegundoApellido = "leternoft",
                             CorreoElectronico = "leternoft@gmail.com",
                             Telefono = "87657976",
                             Sexo = "Masculino",
                             Cedula = "023-090294-5000F",
                             Direccion = "Laureles Sur calle del ministerio 2 C arriba",
                             Foto = null,
                             FechaIngreso = DateTime.Now,
                             EsEmpresa = true,
                             NombreDeLaEmpresa = "freya Cosmetics",
                             RUC = "J0130867567891"
                         });


            context.Disponibilidad.AddOrUpdate(p => p.Id,
                  new Models.Disponibilidad
                  {
                      Descripcion = "Tiempo Completo",
                      EstadoDeLaDisponibilidad = true
                  },
                    new Models.Disponibilidad
                    {
                        Descripcion = "Medio Tiempo",
                        EstadoDeLaDisponibilidad = true
                    },
                     new Models.Disponibilidad
                     {
                         Descripcion = "Horas Libres",
                         EstadoDeLaDisponibilidad = true
                     });

            context.VelocidadDeConexion.AddOrUpdate(p => p.Id,
                new Models.VelocidadDeConexion
                {
                    Descripcion = "Lento",
                    Estado = true
                },
                new Models.VelocidadDeConexion
                {
                    Descripcion = "Medio",
                    Estado = true
                },
                         new Models.VelocidadDeConexion
                         {
                             Descripcion = "Rapido",
                             Estado = true
                         },

                 new Models.VelocidadDeConexion
                 {
                     Descripcion = "Super Rapido",
                     Estado = true
                 }
                 );

            context.TiposDeServicio.AddOrUpdate(p => p.Id,
    new Models.TipoDeServicio
    {
        Id = 1,
        DescripcionTipoDeServicio = "Mandaditos",
        EstadoTipoDeServicio = true
    },
    new Models.TipoDeServicio
    {
        Id = 2,
        DescripcionTipoDeServicio = "Gestion de Paquetería",
        EstadoTipoDeServicio = true
    },
    new Models.TipoDeServicio
    {
        Id = 3,
        DescripcionTipoDeServicio = "Gestion Bancaria",
        EstadoTipoDeServicio = true
    });

            context.Servicios.AddOrUpdate(p => p.Id,
new Models.Servicio
{
Id = 1,
DescripcionDelServicio = "Ir a traerme las llaves",
TipoDeServicioId = 1,
Estado = true
},
new Models.Servicio
{
Id = 2,
DescripcionDelServicio = "Ir a traerme el cargador de mi laptop",
TipoDeServicioId = 1,
Estado = true
},
new Models.Servicio
{
Id = 3,
DescripcionDelServicio = "Entrega de perfumes",
TipoDeServicioId = 2,
Estado = true
});

            context.TiposDePago.AddOrUpdate(p => p.Id,
new Models.TipoDePago
{
Id = 1,
Descripcion = "Efectivo",
EstadoTipoDePago = true
},
new Models.TipoDePago
{
    Id = 2,
    Descripcion = "Transferencia electrónica",
    EstadoTipoDePago = true
},
new Models.TipoDePago
{
    Id = 3,
    Descripcion = "Crédito",
    EstadoTipoDePago = true
});


            context.CostoGestionBancaria.AddOrUpdate(
                p => p.Id,
                new Models.CostoGestionBancaria
                {
                    Id = 0,
                    FechaDeInicio = DateTime.Parse("01/08/2021"),
                    FechaDeFin = DateTime.Parse("30/09/2024"),
                    Descripcion = "Sin Especificar",
                    MontoDesde = 1,
                    MontoHasta = 1,
                    Porcentaje = 0,
                    TipoDeServicioId = 3,
                    Estado = true
                },
                new Models.CostoGestionBancaria
                {
                    Id = 1,
                    FechaDeInicio = DateTime.Parse("01/08/2021"),
                    FechaDeFin = DateTime.Parse("30/09/2025"),
                    Descripcion = "Costo minimo de Gestion Bancaria",
                    MontoDesde = 1,
                    MontoHasta = 5000.99M,
                    Porcentaje = 2.4f,
                    TipoDeServicioId = 3,
                    PrecioDeRecargo = 20,
                    Estado = true
                },
                new Models.CostoGestionBancaria
                {
                    Id = 2,
                    FechaDeInicio = DateTime.Parse("01/08/2021"),
                    FechaDeFin = DateTime.Parse("30/09/2026"),
                    Descripcion = "Costo maximo de Gestion Bancaria",
                    MontoDesde = 5001,
                    MontoHasta = 10000,
                    Porcentaje = 3,
                    TipoDeServicioId = 3,
                    PrecioDeRecargo = 20,
                    Estado = true
                });

            context.Costos.AddOrUpdate(p => p.Id,
    new Models.Costo
    {
        FechaDeInicio = DateTime.Parse("01/08/2021"),
        FechaDeFin = DateTime.Parse("30/09/2027"),
        Descripcion = "Costo de Mandaditos",
        CostoDeGasolina = 20, //float.Parse("20"),
        CostoDeAsistencia = 20,
        CostoDeMotorizado = 20,
        DistanciaBase = 3,
        PrecioPorKm = 3,
        TipoDeServicioId = 1,
        EstadoDelCosto = true,
        PrecioDeRecargo = 20
    },
    new Models.Costo
    {
        FechaDeInicio = DateTime.Parse("01/08/2021"),
        FechaDeFin = DateTime.Parse("30/09/2028"),
        Descripcion = "Costo de Gestion de Paquetería",
        CostoDeGasolina = 20, //float.Parse("20"),
        CostoDeAsistencia = 20,
        CostoDeMotorizado = 20,
        DistanciaBase = 3,
        PrecioPorKm = 3,
        TipoDeServicioId = 2,
        EstadoDelCosto = true,
        PrecioDeRecargo = 20
    }
    );

            context.Monedas.AddOrUpdate(p => p.Id,
   new Models.Moneda
   {
       Id = 1,
       Abreviatura = "C$",
       NombreDeMoneda = "Cordobas",
       Estado = true
   },
   new Models.Moneda
   {
       Id = 2,
       Abreviatura = "$",
       NombreDeMoneda = "Dollar",
       Estado = false
   });


            //context.Motorizados.AddOrUpdate(
            //    p => p.Id,
            //           new Models.Motorizado
            //           {
            //               PrimerNombre = "Walter",
            //               SegundoNombre = "Lucindro",
            //               PrimerApellido = "Valladares",
            //               SegundoApellido = "Valles",
            //               CorreoElectronico = "Valladares@gmail.com",
            //               Telefono = "87987658",
            //               Sexo = "Masculino",
            //               Cedula = "003-080797-7500F",
            //               Direccion = "Laureles Sur calle del ministerio 2 C arriba",
            //               Foto = null,
            //               FechaIngreso = DateTime.Now,
            //               EsAfiliado = true,
            //               FechaDeAfiliacion = DateTime.Now,
            //               EstadoDeAfiliado = (short)EstadoDeAfiliadoEnum.Afiliado,
            //               EstadoDelMotorizado = true
            //           },
            //                new Models.Motorizado
            //                {
            //                    PrimerNombre = "Hilson",
            //                    SegundoNombre = "Partizano",
            //                    PrimerApellido = "Bendremhar",
            //                    SegundoApellido = "Bounthy",
            //                    CorreoElectronico = "Bounthy@gmail.com",
            //                    Telefono = "89536536",
            //                    Sexo = "Masculino",
            //                    Cedula = "021-010191-0000F",
            //                    Direccion = "Laureles Sur calle del ministerio 2 C arriba",
            //                    Foto = null,
            //                    FechaIngreso = DateTime.Now,
            //                    EsAfiliado = false,
            //                    FechaDeAfiliacion =DateTime.Parse("01/01/1900 00:00:00"),//default date 
            //                    EstadoDeAfiliado = (short)EstadoDeAfiliadoEnum.NoAplica,
            //                    EstadoDelMotorizado = true
            //                }
            //           ) ;
        }
    }
}
