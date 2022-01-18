using MandaditosExpress.Models;
using MandaditosExpress.Models.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MandaditosExpress.API
{
    [AllowAnonymous]
    public class LoginWSController : Controller
    {
        [HttpPost]
        public JsonResult Login(string user="", string pass="")
        {
            RespuestaLogin resp = new RespuestaLogin();

            var result = System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>().PasswordSignIn(user, pass, false, false);

            if (result == SignInStatus.Success)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var MandaditosDb = new MandaditosDB();

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
                        var Person = MandaditosDb.Personas.FirstOrDefault(it=> it.CorreoElectronico == UserInDb.UserName);
                        var PersonName = Person != null ? Person?.CorreoElectronico.ToString() : UserInDb.UserName.ToString();

                        resp.Mensaje = "Hola, Bienvenido " + PersonName + "!";
                    }
                    else
                    {
                        resp.IsConfirmed = false;
                        resp.Mensaje = "Estimado usuario debes confirmar tu correo electronico primero!";
                    }
                }
            }
            else
            {
                resp.Exito = false;
                resp.IsConfirmed = false;
                resp.Mensaje = "Usuario o Contraseña Incorrectos";
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }
    }
}