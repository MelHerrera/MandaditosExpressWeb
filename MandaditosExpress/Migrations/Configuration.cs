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

            context.Motorizados.AddOrUpdate(
                p => p.Id,
                       new Models.Motorizado
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
                           EsAfiliado = true,
                           FechaDeAfiliacion = DateTime.Now,
                           EstadoDeAfiliado = (short)EstadoDeAfiliadoEnum.Afiliado,
                           EstadoDelMotorizado = true
                       },
                            new Models.Motorizado
                            {
                                PrimerNombre = "Hilson",
                                SegundoNombre = "Partizano",
                                PrimerApellido = "Bendremhar",
                                SegundoApellido = "Bounthy",
                                CorreoElectronico = "Bounthy@gmail.com",
                                Telefono = "89536536",
                                Sexo = "Masculino",
                                Cedula = "021-010191-0000F",
                                Direccion = "Laureles Sur calle del ministerio 2 C arriba",
                                Foto = null,
                                FechaIngreso = DateTime.Now,
                                EsAfiliado = false,
                                FechaDeAfiliacion =DateTime.Parse("01/01/1998 00:00:00"),//default date 
                                EstadoDeAfiliado = (short)EstadoDeAfiliadoEnum.NoAplica,
                                EstadoDelMotorizado = true
                            }
                       ) ;
        }
    }
}
