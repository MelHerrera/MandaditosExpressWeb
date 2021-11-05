using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;
using MandaditosExpress.Models.Extensions;
using MandaditosExpress.Models.Utileria;
using MandaditosExpress.Models.ViewModels;
using MandaditosExpress.Services;

namespace MandaditosExpress.Controllers
{
    [Authorize]
    public class EnviosController : Controller
    {
        private readonly IMapper _mapper;
        private MandaditosDB db;
        private Utileria Utileria;
        private CotizacionServices CotizacionServices;
        private CostoServices CostoServices;

        public EnviosController(IMapper mapper)
        {
            _mapper = mapper;
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
            var EnvioViewModel = new SolicitudEnvioViewModel();
            EnvioViewModel.TiposDeServicio = _mapper.Map<ICollection<TipoDeServicioViewModel>>(db.TiposDeServicio).ToList();
            EnvioViewModel.Servicios = _mapper.Map<ICollection<ServicioViewModel>>(db.Servicios).ToList();
            EnvioViewModel.TiposDePago = _mapper.Map<ICollection<TipoDePagoViewModel>>(db.TiposDePago).ToList();

            var gestion = db.TiposDeServicio.FirstOrDefault(x => x.DescripcionTipoDeServicio.ToUpper().Contains("BANC"));
            EnvioViewModel.GestionBancariaId = gestion != null ? gestion.Id : -1;

            var cotizacion = db.Cotizaciones.FirstOrDefault(it => it.Id == CotizacionId && it.FechaDeValidez >= DateTime.Now);//Buscar una cotizacion Asociada

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

        // GET: Envios/Asignacion
        public ActionResult Asignacion(int? id)
        {
            var Envio = db.Envios.FirstOrDefault(it => it.Id == id);

            if (id == null || Envio==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var AsignacionViewModel = new AsignarMotorizadoViewModel();

            AsignacionViewModel.Envio = _mapper.Map<EnvioViewModel>(Envio);

            //Los motorizados que se pueden asignar son los que estan activos a la espera de un pedido, que no hayan sido denegados y que su proceso de afiliacion no este en solicitud apenas
            var Motorizados = db.Motorizados.Where(x => x.EstadoDelMotorizado == (short)EstadoDeMotorizadoEnum.Activo && x.EstadoDeAfiliado != (short)EstadoDeAfiliadoEnum.Denegado && x.EstadoDeAfiliado != (short)EstadoDeAfiliadoEnum.Solicitud);

            //para efectos de prueba se estara trabajando todos con los motorizados sin importar su estado
            var MotorizadosPrueba = db.Motorizados.ToList();
            AsignacionViewModel.Motorizados = _mapper.Map<ICollection<MotorizadoViewModel>>(MotorizadosPrueba).ToList();

            return View(AsignacionViewModel);
        }

        // Post: Envios/Asignacion
        [HttpPost]
        public ActionResult AsignacionConfirmed(int? MotorizadoId, int? EnvioId)
        {

            var Motorizado = db.Motorizados.FirstOrDefault(it=> it.Id == MotorizadoId);
            //validaciones sobre el motorizado seleccionado
            if(Motorizado==null)
                return Json(new { message = "El motorizado seleccionado es invalido", exito = false }, JsonRequestBehavior.AllowGet);
            if(Motorizado.EstadoDelMotorizado != (short)EstadoDeMotorizadoEnum.Activo)
                return Json(new { message = "El motorizado no se encuentra disponible para realizar este envio", exito = false }, JsonRequestBehavior.AllowGet);

            var Envio = db.Envios.FirstOrDefault(it=> it.Id == EnvioId);

            //validaciones sobre el motorizado seleccionado
            if (Envio == null || Envio.MontoTotalDelEnvio<=0 || Envio.DistanciaEntregaRecep<=0)
                return Json(new { message = "El envio seleccionado es invalido", exito = false }, JsonRequestBehavior.AllowGet);

            Envio.MotorizadoId = Motorizado.Id;
            Envio.EstadoDelEnvio = (short) EstadoDelEnvioEnum.EnProceso;

            db.Entry(Envio).State = EntityState.Modified;

            if(db.SaveChanges() > 0)
            {
                //si todo esta correcto, como se esta haciendo manual la asignacion entonces manualmente debemos cambiar el estado del motorizado
                Motorizado.EstadoDelMotorizado = (short)EstadoDeMotorizadoEnum.Ocupado;

                return Json(new { message = "Se ha realizado con exito la asignación", exito = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "Ha ocurrido un error en la asignación del motorizado", exito = false }, JsonRequestBehavior.AllowGet);
        }

        // POST: Envios/Createss
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create([Bind(Include = "Id,DescripcionDeEnvio,FechaDelEnvio,LugarOrigen,LugarDestino,DistanciaEntregaRecep,NombresDelReceptor,CedulaDelReceptor,Peso,MontoDeDinero,TelefonoDelReceptor,EsUrgente,DebeRegresarATienda,DebeRecibirDinero,MontoARecibir,DebeRecibirCambio,MontoCambio,EstadoDelEnvio,ClienteId,TipoDePagoId,TipoDeServicioId,ServicioId, Servicio")] SolicitudEnvioViewModel envio)
        {
            //capturar el valor del query string en el envioviewmodel pero que no lo valide, ya que, no son obligatorios
            ModelState.Remove("envio.Servicio.DescripcionDelServicio");
            ModelState.Remove("envio.Cotizacion.DescripcionDeCotizacion");

            if (ModelState.IsValid)
            {
                var InvalidMessage = CotizacionServices.ValidarDatosCotizacion(envio.TipoDeServicioId, envio.DistanciaEntregaRecep, envio.MontoDeDinero, envio.FechaDelEnvio);

                if(!string.IsNullOrEmpty(InvalidMessage))//si hay algun mensaje de error devolverlo
                    return Json(new { message = InvalidMessage, exito = false }, JsonRequestBehavior.AllowGet);

                var CostoAsociado = CostoServices.ValidarVigenciaCostos(envio.TipoDeServicioId, envio.FechaDelEnvio, envio.MontoDeDinero);

                if (CostoAsociado == null)//si hasta este punto sigue sin encontrarse un costo vigente asociado significa que no hay un costo para el tipo de servicio pasado como parametros
                    return Json(new { message = "No se encontró ningun costo vigente asociado al tipo de servicio seleccionado, para mayor información contactese con atención al cliente", exito = false }, JsonRequestBehavior.AllowGet);
                else
                {
                    var mEnvio = new Envio();

                    //si es un nuevo servicio guardarlo antes de ocupar servicioId
                    if (envio.ServicioId == -1)
                    {
                        envio.Servicio.TipoDeServicioId = envio.TipoDeServicioId;
                        envio.Servicio.Estado = true;

                        var service = _mapper.Map<Servicio>(envio.Servicio);
                        db.Servicios.Add(service);
                        db.SaveChanges();

                        //Una vez guardado el nuevo servicio actualizar la informacion del envio
                        envio.ServicioId = service.Id;
                    }

                    //si el envio viene mediante una cotizacion trabajar con esa cotizacion primero, por si corrompieron la informacion en la vista, mas largo el proceso pero mas seguro
                    var cotizacion = db.Cotizaciones.FirstOrDefault(it => it.Id == envio.CotizacionId);

                    if (cotizacion != null)//si el envio es mediante cotizacion entonces ocupar los datos de la cotizacion, porque en la vista del envio los pudieron corromper
                    {
                        if (cotizacion.MontoTotal > 0)
                        {
                            //datos que vienen por cotizacion
                            mEnvio.CotizacionId = cotizacion.Id;
                            mEnvio.TipoDeServicioId = cotizacion.TipoDeServicioId;
                            mEnvio.LugarOrigen = cotizacion.LugarOrigen;
                            mEnvio.LugarDestino = cotizacion.LugarDestino;
                            mEnvio.MontoDeDinero = cotizacion.MontoDeDinero;
                            mEnvio.EsUrgente = cotizacion.EsEspecial;
                            mEnvio.DistanciaEntregaRecep = cotizacion.DistanciaOrigenDestino;
                            mEnvio.MontoTotalDelEnvio = cotizacion.MontoTotal;


                            //si es un nuevo servicio 

                            //datos que siempre se tomaran del envio
                            mEnvio.DescripcionDeEnvio = envio.DescripcionDeEnvio;
                            mEnvio.FechaDelEnvio = envio.FechaDelEnvio;
                            mEnvio.TipoDePagoId = envio.TipoDeServicioId;
                            mEnvio.ServicioId = envio.ServicioId;
                            mEnvio.NombresDelReceptor = envio.NombresDelReceptor;
                            mEnvio.CedulaDelReceptor = envio.CedulaDelReceptor;
                            mEnvio.TelefonoDelReceptor = envio.TelefonoDelReceptor;
                            mEnvio.Peso = envio.Peso;
                            mEnvio.DebeRegresarATienda = envio.DebeRegresarATienda;
                            mEnvio.DebeRecibirDinero = envio.DebeRecibirDinero;
                            mEnvio.MontoARecibir = envio.MontoARecibir;
                            mEnvio.DebeRecibirCambio = envio.DebeRecibirCambio;
                            mEnvio.MontoCambio = envio.MontoCambio;
                            mEnvio.EstadoDelEnvio = envio.EstadoDelEnvio;
                            mEnvio.ClienteId = envio.ClienteId;
                        }
                    }
                    else
                    {
                            var MontoTotalDelEnvio = CotizacionServices.Cotizar(envio.TipoDeServicioId, envio.FechaDelEnvio, envio.MontoDeDinero, envio.DistanciaEntregaRecep, envio.EsUrgente);

                            if (MontoTotalDelEnvio > 0)
                            {
                                mEnvio = new Envio
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
                            }
                    }

                    //despues de asignada la informacion correspondiente
                    db.Envios.Add(mEnvio);

                    if (db.SaveChanges() > 0)
                        return Json(new { exito = true, message = "Estimado cliente se ha realizado con éxito la solicitud de su envío" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { exito = false, message = "No se ha podido guardar tu solicitud, para mayor información contactese con atención al cliente" }, JsonRequestBehavior.AllowGet);
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
            var CostoAsociado = CostoServices.ValidarVigenciaCostos(TipoDeServicioId, Fecha, MontoGestion);

            if (CostoAsociado == null)//si hasta este punto sigue sin encontrarse un costo vigente asociado significa que no hay un costo para el tipo de servicio pasado como parametros
                return Json(new { message = "No se encontró ningun costo asociado al tipo de servicio seleccionado, para mayor información contactese con atención al cliente", exito = false }, JsonRequestBehavior.AllowGet);


            var MontoTotal = CotizacionServices.Cotizar(TipoDeServicioId, Fecha, MontoGestion, Distancia, Urgente);

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
