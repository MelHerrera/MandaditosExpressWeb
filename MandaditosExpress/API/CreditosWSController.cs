using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Extensions;
using Newtonsoft.Json;

namespace MandaditosExpress.API
{
    public class CreditosWSController : Controller
    {
        private MandaditosDB db;
        private readonly IMapper mapper;

        public CreditosWSController(IMapper mapper)
        {
            db = new MandaditosDB();
            this.mapper = mapper;
        }

        // GET: CreditosWS
        public JsonResult GetCreditosDelCliente(int ClienteId)
        {
            try
            {
                var Response = new ResponseWsListaDeCreditos();
                var Cliente = db.Creditos.Find(ClienteId);

                if (Cliente is null)
                {
                    Response.Mensaje = "Ha proporcionado un cliente inválido";
                }
                else
                {
                    var creditos = db.Creditos.Where(it => it.ClienteId == ClienteId).ToList();
                    Response.Creditos = mapper.Map<ICollection<ResponseWsCredito>>(creditos).ToList();
                    Response.Exito = true;
                    Response.Mensaje = "Se cargó la información correctamente";
                }

                return Json(Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //no revelar información confidencial
                return Json(new ResponseWsListaDeCreditos { Mensaje = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}