using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MandaditosExpress.API
{
    public class PagosWSController : Controller
    {
        private MandaditosDB db;
        private readonly IMapper mapper;

        public PagosWSController(IMapper mapper)
        {
            db = new MandaditosDB();
            this.mapper = mapper;
        }

        // GET: PagosWS
        public JsonResult GetPagosDelCliente(int ClienteId)
        {
            try
            {
                var Response = new ResponseWsListaDePagos();
                var Cliente = db.Clientes.FirstOrDefault(x=> x.Id == ClienteId);

                if (Cliente is null)
                {
                    Response.Mensaje = "Ha proporcionado un cliente inválido";
                }
                else
                {
                    var Pagos = db.Pagos.Where(x => x.Envio != null ? x.Envio.ClienteId == ClienteId : x.Credito.ClienteId == ClienteId).ToList();
                    Response.Pagos = mapper.Map<ICollection<ResponseWsPago>>(Pagos).ToList();
                    Response.Exito = true;
                    Response.Mensaje = "Se cargó la información correctamente";
                }

                return Json(Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //no revelar información confidencial
                return Json(new ResponseWsListaDePagos { Mensaje = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}