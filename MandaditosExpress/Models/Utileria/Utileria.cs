using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MandaditosExpress.Models.Utileria
{
    public class Utileria
    {
        private MandaditosDB db = new MandaditosDB();
        private ApplicationDbContext userContext;

        public Utileria()
        {
            userContext = new ApplicationDbContext();
        }

        public Persona BuscarPersonaPorUsuario(string UserName)
        {
            return db.Personas.DefaultIfEmpty(null).FirstOrDefault(x => x.CorreoElectronico == UserName);
        }

        public Cliente GetClienteByUser(string CurrentUser)
        {
            return db.Clientes.FirstOrDefault(c => c.CorreoElectronico == CurrentUser);
        }
        public Byte[] getImageBytes(HttpRequestBase request)
        {
            if (request.Files.Count > 0)
            {
                HttpPostedFileBase imagenBase = request.Files[0];

                if (imagenBase.ContentLength > 0)
                {
                    WebImage imagen = new WebImage(imagenBase.InputStream);
                    return imagen.GetBytes();
                }
            }
            return null;
        }

        public string GetRolesDeUsuario(string UserName)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            //extraer los datos del usuario
            var UserInDb = UserManager.FindByEmail(UserName);

            if (UserInDb != null)
            {
                List<string> rolesName = UserManager.GetRoles(UserInDb.Id).ToList();
                return string.Join(",", rolesName);
            }
            else
                return "";
        }
    }
}