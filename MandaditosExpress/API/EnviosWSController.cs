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
    public class EnviosWSController : Controller
    {
        private MandaditosDB db;
        private readonly IMapper mapper;

        public EnviosWSController(IMapper mapper)
        {
            db = new MandaditosDB();
            this.mapper = mapper;
        }

        // GET: EnviosWS
        public JsonResult GetEnviosDelCliente(int ClienteId)
        {
            try
            {
                var Response = new ResponseWsListaEnvios();

                var Cliente = db.Clientes.Find(ClienteId);

                if(Cliente == null)
                {
                    Response.Mensaje = "Ha proporcionado un cliente inválido";
                    Response.Exito = false;
                }
                else
                {
                    var envios = db.Envios.Where(it=> it.ClienteId == Cliente.Id).ToList();
                    Response.Envios = mapper.Map<ICollection<ResponseWsEnvio>>(envios).ToList();
                    Response.Exito = true;
                }

                return Json(Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //no revelar información confidencial
                return Json(new ResponseWsListaDeCotizaciones { Mensaje = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}