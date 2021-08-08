using System;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MandaditosExpress.Models.Utileria
{
    public class Utileria
    {
        private MandaditosDB db = new MandaditosDB();

        public Persona BuscarPersonaPorUsuario(string UserName)
        {
            return db.Personas.DefaultIfEmpty(null).FirstOrDefault(x => x.CorreoElectronico == UserName);
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
    }
}