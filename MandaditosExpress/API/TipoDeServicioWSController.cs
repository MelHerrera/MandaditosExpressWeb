using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MandaditosExpress.API
{
    public class TipoDeServicioWSController : Controller
    {
        private MandaditosDB db;
        private readonly IMapper mapper;

        public TipoDeServicioWSController(IMapper mapper)
        {
            db = new MandaditosDB();
            this.mapper = mapper;
        }

        // GET: TipoDeServicioWS
        public ActionResult GetAllTiposDeServicio()
        {
            try
            {
                var Response = new ResponseWsListaTiposDeServicio();

                Response.Exito = true;
                Response.TiposDeServicio = mapper.Map<ICollection<ResponseWsTipoDeServicio>>(db.TiposDeServicio.Where(it => it.EstadoTipoDeServicio)).ToList();
                Response.Mensaje = "Se cargo la información correctamente";

                return Json(Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //no revelar información confidencial
                return Json(new ResponseWsListaTiposDeServicio { Mensaje = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}