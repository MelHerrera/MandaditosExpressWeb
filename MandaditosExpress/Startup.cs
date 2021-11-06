using MandaditosExpress.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(MandaditosExpress.Startup))]
namespace MandaditosExpress
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CrearRoles();
        }

        private void CrearRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            MandaditosDB db = new MandaditosDB();

            var AdmRoles = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var AdmUsuarios = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            IdentityRole Rol = new IdentityRole();

            if (!AdmRoles.RoleExists("Admin"))
            {
                Rol = new IdentityRole();
                Rol.Name = "Admin";
                AdmRoles.Create(Rol);

                var usuario = new ApplicationUser();
                usuario.UserName = "melkinantonioh@gmail.com";
                usuario.Email = "melkinantonioh@gmail.com";
                usuario.EmailConfirmed = true;
                

                var resultado = AdmUsuarios.Create(usuario, "Mel#123");
                if (resultado.Succeeded)
                {
                    AdmUsuarios.AddToRole(usuario.Id, "Admin");
                    //Crearle los datos correspondientes como persona del sistema
                    var Persona = new Persona();
                    Persona.CorreoElectronico = usuario.UserName;
                    Persona.PrimerNombre = "Melkin";
                    Persona.PrimerApellido = "Herrera";
                    Persona.SegundoApellido = "Mendoza";
                    Persona.Telefono = "82415644";
                    Persona.Sexo = "Masculino";
                    Persona.Direccion = "Colonia Miguel Bonilla UNAN";
                    Persona.FechaIngreso = DateTime.Now;

                    db.Personas.Add(Persona);
                    db.SaveChanges();
                }
            }

            if (!AdmRoles.RoleExists("Cliente"))
            {
                Rol = new IdentityRole();
                Rol.Name = "Cliente";
                AdmRoles.Create(Rol);

                var usuario = new ApplicationUser();
                usuario.UserName = "jcastillov@gmail.com";
                usuario.Email = "jcastillov@gmail.com";

                var resultado = AdmUsuarios.Create(usuario, "Jcas#123");
                if (resultado.Succeeded)
                {
                    AdmUsuarios.AddToRole(usuario.Id, "Cliente");
                }
            }


            if (!AdmRoles.RoleExists("Motorizado"))
            {
                Rol = new IdentityRole();
                Rol.Name = "Motorizado";
                AdmRoles.Create(Rol);

                var usuario = new ApplicationUser();
                usuario.UserName = "paty@gmail.com";
                usuario.Email = "paty@gmail.com";
                usuario.EmailConfirmed = true;

                var resultado = AdmUsuarios.Create(usuario, "Paty#123");
                if (resultado.Succeeded)
                {
                    AdmUsuarios.AddToRole(usuario.Id, "Motorizado");
                }
            }
        }
    }
}
