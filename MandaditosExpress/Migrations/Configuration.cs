namespace MandaditosExpress.Migrations
{
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
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
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

        }
    }
}
