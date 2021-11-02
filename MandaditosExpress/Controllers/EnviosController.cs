using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Utileria;
using MandaditosExpress.Models.ViewModels;
using MandaditosExpress.Services;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize]
    public class EnviosController : Controller
    {
        private MandaditosDB db;
        private Utileria Utileria;
        private CotizacionServices CotizacionServices;
        private CostoServices CostoServices;

        public EnviosController()
        {
            db = new MandaditosDB();
            Utileria = new Utileria();
            CotizacionServices = new CotizacionServices(db);
            CostoServices = new CostoServices(db);
        }

        // GET: Envios
        public ActionResult Index()
        {
            var envios = db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio);
            return View(envios.ToList());
        }

        // GET: Envios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envios.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            return View(envio);
        }

        // GET: Envios/Create
        public ActionResult Create(int? CotizacionId)
        {
            var EnvioViewModel = new EnvioViewModel();

            var cotizacion = db.Cotizaciones.FirstOrDefault(it=> it.Id == CotizacionId && it.FechaDeValidez>= DateTime.Now);//Buscar una cotizacion Asociada

            if (cotizacion != null)//si el envio se va a realizar mediante una cotizacion
            {
                EnvioViewModel.CotizacionId = cotizacion.Id;
                EnvioViewModel.TipoDeServicioId = cotizacion.TipoDeServicioId;
                EnvioViewModel.LugarOrigen = cotizacion.LugarOrigen;
                EnvioViewModel.LugarDestino = cotizacion.LugarDestino;
                EnvioViewModel.MontoDeDinero = cotizacion.MontoDeDinero;
                EnvioViewModel.EsUrgente = cotizacion.EsEspecial;
                EnvioViewModel.DistanciaEntregaRecep = cotizacion.DistanciaOrigenDestino;
                EnvioViewModel.MontoTotalDelEnvio = cotizacion.MontoTotal;
            }

            //sacar el Id del Cliente que esta haciendo la solicitud del envio
            var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;
            var Cliente = Utileria.GetClienteByUser(CurrentUser);

            EnvioViewModel.ClienteId = Cliente != null ? Cliente.Id : -1;

            //sino tiene credito ocultar de la lista de tipos de pagos la ópcion de creditos
            var TieneCredito = db.Creditos.Where(it => it.FechaDeInicio <= DateTime.Today && it.FechaDeVencimiento >= DateTime.Today && it.ClienteId == EnvioViewModel.ClienteId).Count() > 0;

            if (!TieneCredito)
            {
                //Buscar en los tipos de pagos si existe el Id del credito
                var Credito = db.TiposDePago.FirstOrDefault(it => it.Descripcion.ToUpper().StartsWith("CRED") || it.Descripcion.ToUpper().StartsWith("CRÉD"));
                var CreditoId = Credito != null ? Credito.Id : -1;

                //excluir ese registro de los tipos de pagos
                EnvioViewModel.TiposDePago = EnvioViewModel.TiposDePago.Where(it => it.Id != CreditoId).ToList();
            }

            return View(EnvioViewModel);
        }

        // POST: Envios/Createss
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(EnvioViewModel envio)
        {
            if (ModelState.IsValid)
            {
                var CostoAsociado = CostoServices.ValidarVigenciaCostos(envio.TipoDeServicioId, envio.FechaDelEnvio);

                if (CostoAsociado == null)//si hasta este punto sigue sin encontrarse un costo vigente asociado significa que no hay un costo para el tipo de servicio pasado como parametros
                    return Json(new { message = "No se encontró ningun costo vigente asociado al tipo de servicio seleccionado, para mayor información contactese con atención al cliente", exito = false }, JsonRequestBehavior.AllowGet);
                else
                {
                    var MontoTotalDelEnvio = CotizacionServices.Cotizar(envio.TipoDeServicioId, envio.FechaDelEnvio, envio.MontoDeDinero, envio.DistanciaEntregaRecep, envio.EsUrgente);

                    if (MontoTotalDelEnvio > 0)
                    {
                        var mEnvio = new Envio
                        {
                            DescripcionDeEnvio = envio.DescripcionDeEnvio,
                            FechaDelEnvio = envio.FechaDelEnvio,
                            TipoDePagoId = envio.TipoDeServicioId,
                            TipoDeServicioId = envio.TipoDeServicioId,
                            ServicioId = envio.ServicioId,
                            NombresDelReceptor = envio.NombresDelReceptor,
                            CedulaDelReceptor = envio.CedulaDelReceptor,
                            TelefonoDelReceptor = envio.TelefonoDelReceptor,
                            MontoDeDinero = envio.MontoDeDinero,
                            EsUrgente = envio.EsUrgente,
                            Peso = envio.Peso,
                            DebeRegresarATienda = envio.DebeRegresarATienda,
                            DebeRecibirDinero = envio.DebeRecibirDinero,
                            MontoARecibir = envio.MontoARecibir,
                            DebeRecibirCambio = envio.DebeRecibirCambio,
                            MontoCambio = envio.MontoCambio,
                            LugarOrigen = envio.LugarOrigen,
                            LugarDestino = envio.LugarDestino,
                            DistanciaEntregaRecep = envio.DistanciaEntregaRecep,
                            EstadoDelEnvio = envio.EstadoDelEnvio,
                            ClienteId = envio.ClienteId,
                            CotizacionId = envio.CotizacionId,
                            MontoTotalDelEnvio = MontoTotalDelEnvio
                        };

                        db.Envios.Add(mEnvio);

                        if(db.SaveChanges() > 0)
                            return Json(new { exito = true, message="Estimado cliente se ha realizado con éxito la solicitud de su envío" }, JsonRequestBehavior.AllowGet);
                        else
                            return Json(new { exito = false, message = "No se ha podido guardar tu solicitud, para mayor información contactese con atención al cliente" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(new { message = "Ha ocurrido un error procesando su solicitud!", exito = false }, JsonRequestBehavior.AllowGet);
        }

        // GET: Envios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envios.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsistenteId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.AsistenteId);
            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.ClienteId);
            ViewBag.MotocicletaId = new SelectList(db.Motocicletas, "Id", "Placa", envio.MotocicletaId);
            ViewBag.MotorizadoId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.MotorizadoId);
            //ViewBag.ServicioId = new SelectList(db.Servicios, "Id", "DescripcionDelServicio", envio.ServicioId);
            return View(envio);
        }

        // POST: Envios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DescripcionDeEnvio,FechaDelEnvio,DireccionDeRecepcion,DireccionDeEntrega,DistanciaEntregaRecep,NombresDelReceptor,ApellidosDelReceptor,CedulaDelReceptor,Ancho,Alto,Peso,MontoDeDinero,TelefonoDelReceptor,FechaDeEntrega,EsUrgente,HoraDeEntrega,PrecioDeRecargo,EstadoDelEnvio,MotocicletaId,AsistenteId,ClienteId,MotorizadoId,Credito,ServicioId")] Envio envio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(envio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsistenteId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.AsistenteId);
            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.ClienteId);
            ViewBag.MotocicletaId = new SelectList(db.Motocicletas, "Id", "Placa", envio.MotocicletaId);
            ViewBag.MotorizadoId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.MotorizadoId);
            //ViewBag.ServicioId = new SelectList(db.Servicios, "Id", "DescripcionDelServicio", envio.ServicioId);
            return View(envio);
        }

        // GET: Envios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envios.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            return View(envio);
        }

        // POST: Envios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Envio envio = db.Envios.Find(id);
            db.Envios.Remove(envio);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult FiltrarServicio(int id)
        {

            if (id > 0)
            {
                List<Servicio> servicios = db.Servicios.Where(it => it.TipoDeServicioId == id).ToList()
                    .Select(y => new Servicio()
                    {
                        Id = y.Id,
                        DescripcionDelServicio = y.DescripcionDelServicio,
                        Estado = y.Estado,
                        TipoDeServicioId = y.TipoDeServicioId
                    }).ToList();

                return Json(new { exito = true, data = servicios }, JsonRequestBehavior.AllowGet);

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CostoDelEnvio(int TipoDeServicioId, DateTime Fecha, float Distancia, bool Urgente, decimal MontoGestion)
        {
            var MontoTotal = CotizacionServices.Cotizar(TipoDeServicioId, Fecha, MontoGestion, Distancia, Urgente);

            var CostoAsociado = CostoServices.ValidarVigenciaCostos(TipoDeServicioId, Fecha);

            if (CostoAsociado == null)//si hasta este punto sigue sin encontrarse un costo vigente asociado significa que no hay un costo para el tipo de servicio pasado como parametros
                return Json(new { message = "No se encontró ningun costo asociado al tipo de servicio seleccionado, para mayor información contactese con atención al cliente", exito = false }, JsonRequestBehavior.AllowGet);


            if (MontoTotal > 0)
                return Json(new { data = MontoTotal, exito = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { message = "Ha sucedido un inconveniente al realizar tu cotización, para mayor información contactese con atención al cliente", exito = false }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
