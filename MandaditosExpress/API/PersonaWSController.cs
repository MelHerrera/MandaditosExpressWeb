using MandaditosExpress.Models;
using MandaditosExpress.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MandaditosExpress.API
{
    public class PersonaWSController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

       public JsonResult GetImagePerfilPersona(int PersonaId)
        {
            try
            {
                var Response = new ResponseWsImagenPerfil();

                var Persona = db.Personas.Find(PersonaId);

                if (Persona is null)
                    Response.Mensaje = "El identificador proporcionado es inválido";
                else
                {
                    Response.Imagen = Persona.Foto;
                    Response.Mensaje = "Se cargo correctamente la información";
                    Response.Exito = true;
                }
                return Json(Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //no revelar información confidencial
                return Json(new ResponseWsImagenPerfil { Mensaje = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}