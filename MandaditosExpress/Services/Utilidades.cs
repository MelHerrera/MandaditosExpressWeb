
using MandaditosExpress.Models;
using MandaditosExpress.Models.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MandaditosExpress.Services
{
    public static class Utilidades
    {
        public static int MinGestionBancaria = 120;
        public static int MaxGestionBancaria = 10000;

        public static RespuestaLogin Logeado(string user, string pass)
        {
            var resp = new RespuestaLogin();

            //verificar si existe pero sin extrar la informacion correspondiente
            var result = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>().PasswordSignIn(user, pass, false, false);

            if (result == SignInStatus.Success)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                //extraer los datos del usuario
                var UserInDb = UserManager.FindByEmail(user);

                if (UserInDb != null)
                {
                    resp.Exito = true;
                    List<string> rolesName = UserManager.GetRoles(UserInDb.Id).ToList();
                    resp.Roles = rolesName;

                    if (UserInDb.EmailConfirmed)
                    {
                        resp.IsConfirmed = true;
                        var Name = HttpContext.Current.User.Identity.Name.ToString();
                        resp.Mensaje = "Hola, Bienvenido " + Name + "!";

                        return resp;
                    }
                    else
                    {
                        resp.IsConfirmed = false;
                        var Name = HttpContext.Current.User.Identity.Name.ToString();
                        resp.Mensaje = "Estimado " + Name + " debes confirmar tu corre electronico primero!";
                    }
                }
            }
            resp.Mensaje = "Usuario o Contraseña Incorrectos";
            return resp;
        }
    }
}