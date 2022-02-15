using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;
using MandaditosExpress.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

                if (Cliente == null)
                {
                    Response.Mensaje = "Ha proporcionado un cliente inválido";
                    Response.Exito = false;
                }
                else
                {
                    var envios = db.Envios.Where(it => it.ClienteId == Cliente.Id).ToList();
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

        public JsonResult GetEnviosDelMotorizado(int MotorizadoId, int Estado)
        {
            try
            {
                var Response = new ResponseWsListaEnvios();

                var Motorizado = db.Motorizados.Find(MotorizadoId);

                if (Motorizado == null)
                {
                    Response.Mensaje = "Ha proporcionado un motorizado inválido";
                    Response.Exito = false;
                }
                else
                {
                    var envios = db.Envios.Where(it => it.MotorizadoId == Motorizado.Id);

                    if (Estado != -1)//Si el estado de los envios a filtrar es diferente de Todos
                        envios = envios.Where(it => it.EstadoDelEnvio == Estado);

                    Response.Envios = mapper.Map<ICollection<ResponseWsEnvio>>(envios.ToList()).ToList();
                    Response.Exito = true;
                }

                return Json(Response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //no revelar información confidencial
                return Json(new ResponseWsListaEnvios { Mensaje = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult FinalizarEnvio(int EnvioId)
        {
            try
            {
                var envio = db.Envios.Find(EnvioId);
                var response = new ResponseWsFinalizarEnvio();

                if (envio != null)
                {
                    if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado)
                    {
                        response.Exito = false;
                        response.Mensaje = "Este envio ya se encuentra finalizado";
                    }
                    if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Rechazado)
                    {
                        response.Exito = false;
                        response.Mensaje = "No se puede finalizar este envio, porque se encuentra rechazado";
                    }
                    if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Solicitud)
                    {
                        response.Exito = false;
                        response.Mensaje = "No se puede finalizar este envio, porque apenas se encuentra en solicitud";
                    }
                    if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso)
                    {
                        envio.EstadoDelEnvio = (short)EstadoDelEnvioEnum.Realizado;
                        db.Entry(envio).State = EntityState.Modified;

                        if (db.SaveChanges() > 0)
                        {
                            response.Exito = true;
                            response.Mensaje = "Se realizo con exito la finalización del envio";
                        }
                        else
                        {
                            response.Exito = false;
                            response.Mensaje = "Tuvimos un inconveniente realizando la finalización del envio";
                        }
                    }
                }
                else
                {
                    response.Exito = false;
                    response.Mensaje = "No se pudo realizar la finalización del envio";
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //no revelar información confidencial
                return Json(new ResponseWsFinalizarEnvio { Mensaje = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}