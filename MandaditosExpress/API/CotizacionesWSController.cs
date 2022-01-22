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
    public class CotizacionesWSController : Controller
    {
        private MandaditosDB db;
        private readonly IMapper mapper;

        public CotizacionesWSController(IMapper mapper)
        {
            db = new MandaditosDB();
            this.mapper = mapper;
        }

        // GET: CotizacionesWS
        /// <summary>
        /// Cotizaciones vigentes filtradas por tipo de servicio
        /// </summary>
        /// <param name="TipoDeServicio"></param>
        /// <returns></returns>
        public JsonResult GetCotizacionesDelClienteXTipoServicio(int TipoDeServicio, int ClienteId)
        {
            try
            {
                var Response = new ResponseWsListaDeCotizaciones();
                var Cliente = db.Clientes.FirstOrDefault(x => x.Id == ClienteId);

                if (Cliente is null)
                    Response.Mensaje = "Ha proporcionado un cliente inválido";
                else
                {
                    if (TipoDeServicio == -1)//en la app movil mandar -1 cuando se necesite visualizarlas todas
                    {
                        Response.Cotizaciones = mapper.Map<ICollection<ResponseWsCotizacion>>(db.Cotizaciones.Where(x => x.FechaDeValidez <= DateTime.Now && x.ClienteId == ClienteId)).ToList();
                        Response.Exito = true;
                        Response.Mensaje = "Se cargó exitosamente la información";
                    }
                    else
                    {
                        var Tipo = db.TiposDeServicio.Find(TipoDeServicio);

                        if (Tipo is null)
                            Response.Mensaje = "Ha proporcionado un tipo de servicio inválido";
                        else
                        {
                            Response.Cotizaciones = mapper.Map<ICollection<ResponseWsCotizacion>>(db.Cotizaciones.Where(it => it.TipoDeServicioId == TipoDeServicio && it.FechaDeValidez <= DateTime.Now && it.ClienteId == ClienteId)).ToList();
                            Response.Exito = true;
                            Response.Mensaje = "Se cargó exitosamente la información";
                        }
                    }
                }
                return Json(Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //no revelar información confidencial
                return Json(new ResponseWsListaDeCotizaciones { Mensaje = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}