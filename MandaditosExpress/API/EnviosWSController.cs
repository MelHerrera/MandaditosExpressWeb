using MandaditosExpress.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MandaditosExpress.API
{
    public class EnviosWSController : Controller
    {
        // GET: EnviosWS
        public JsonResult GetEnviosDelCliente(int ClienteId)
        {
            var Response = new ResponseWsListaEnvios();
            return Json(Response, JsonRequestBehavior.AllowGet);
        }
    }
}