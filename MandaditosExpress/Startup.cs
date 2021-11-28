using MandaditosExpress.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

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

            if (!AdmRoles.RoleExists("Asistente"))
            {
                Rol = new IdentityRole();
                Rol.Name = "Asistente";
                AdmRoles.Create(Rol);
            }
        }
    }
}
