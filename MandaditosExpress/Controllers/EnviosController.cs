using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;
using MandaditosExpress.Models.Extensions;
using MandaditosExpress.Models.Utileria;
using MandaditosExpress.Models.ViewModels;
using MandaditosExpress.Services;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin, Cliente")]
    public class EnviosController : Controller
    {
        private readonly IMapper _mapper;
        private MandaditosDB db;
        private Utileria Utileria;
        private CotizacionServices CotizacionServices;
        private CostoServices CostoServices;
        private MotorizadoServices MotorizadoServices;

        public EnviosController(IMapper mapper)
        {
            _mapper = mapper;
            db = new MandaditosDB();
            Utileria = new Utileria();
            CotizacionServices = new CotizacionServices(db);
            CostoServices = new CostoServices(db);
            MotorizadoServices = new MotorizadoServices(db);
        }

        // GET: Envios
        public ActionResult Index()
        {
            var envios = new List<IndexEnvioViewModel>();
            var CurrentUser = new Utileria().GetClienteByUser(User.Identity.Name);
            var ClienteId = CurrentUser != null ? CurrentUser.Id : -1;

            if (User.IsInRole("Cliente"))
                envios = _mapper.Map<ICollection<IndexEnvioViewModel>>(db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio).Where(it => it.ClienteId == ClienteId).ToList()).ToList();
            else
                envios = _mapper.Map<ICollection<IndexEnvioViewModel>>(db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio).ToList()).ToList();

            return View("Index", envios);
        }

        // GET: Envios//Solicitud
        public ActionResult IndexSolicitudes()
        {
            var envios = new List<IndexEnvioViewModel>();
            var CurrentUser = new Utileria().GetClienteByUser(User.Identity.Name);
            var ClienteId = CurrentUser != null ? CurrentUser.Id : -1;

            if (User.IsInRole("Cliente"))
                envios = _mapper.Map<ICollection<IndexEnvioViewModel>>(db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio).Where(it => it.ClienteId == ClienteId && it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Solicitud).ToList()).ToList();
            else
                envios = _mapper.Map<ICollection<IndexEnvioViewModel>>(db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio).Where(it => it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Solicitud).ToList()).ToList();

            return View("Index", envios);
        }

        // GET: Envios//Proceso
        public ActionResult IndexEnProceso()
        {
            var envios = new List<IndexEnvioViewModel>();
            var CurrentUser = new Utileria().GetClienteByUser(User.Identity.Name);
            var ClienteId = CurrentUser != null ? CurrentUser.Id : -1;

            if (User.IsInRole("Cliente"))
                envios = _mapper.Map<ICollection<IndexEnvioViewModel>>(db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio).Where(it => it.ClienteId == ClienteId && it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso).ToList()).ToList();
            else
                envios = _mapper.Map<ICollection<IndexEnvioViewModel>>(db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio).Where(it => it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso).ToList()).ToList();

            return View("Index", envios.ToList());
        }

        // GET: Envios//Finalizados
        public ActionResult IndexFinalizados()
        {
            var envios = new List<IndexEnvioViewModel>();
            var CurrentUser = new Utileria().GetClienteByUser(User.Identity.Name);
            var ClienteId = CurrentUser != null ? CurrentUser.Id : -1;

            if (User.IsInRole("Cliente"))
                envios = _mapper.Map<ICollection<IndexEnvioViewModel>>(db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio).Where(it => it.ClienteId == ClienteId && it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado).ToList()).ToList();
            else
                envios = _mapper.Map<ICollection<IndexEnvioViewModel>>(db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio).Where(it => it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado).ToList()).ToList();

            return View("Index", envios.ToList());
        }

        // GET: Envios//Rechazados
        public ActionResult IndexRechazados()
        {
            var envios = new List<IndexEnvioViewModel>();
            var CurrentUser = new Utileria().GetClienteByUser(User.Identity.Name);
            var ClienteId = CurrentUser != null ? CurrentUser.Id : -1;

            if (User.IsInRole("Cliente"))
                envios = _mapper.Map<ICollection<IndexEnvioViewModel>>(db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio).Where(it => it.ClienteId == ClienteId && it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Rechazado).ToList()).ToList();
            else
                envios = _mapper.Map<ICollection<IndexEnvioViewModel>>(db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio).Where(it => it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Rechazado).ToList()).ToList();

            return View("Index", envios.ToList());
        }

        // GET: Envios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envios.Find(id);

            //verificar si el envio que quiere ver esta en su dominio en el caso que sean clientes
            if (User.IsInRole("Cliente"))
            {
                var Cliente = Utileria.GetClienteByUser(User.Identity.Name);

                if (envio.ClienteId != Cliente.Id)
                {
                    return HttpNotFound();
                }
            }

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
                EnvioViewModel.LugarOrigen = cotizacion.LugarOrigen != null ? _mapper.Map<LugarViewModel>(cotizacion.LugarOrigen) : new LugarViewModel();
                EnvioViewModel.LugarDestino = cotizacion.LugarDestino != null ? _mapper.Map<LugarViewModel>(cotizacion.LugarDestino) : new LugarViewModel();
                EnvioViewModel.MontoDeDinero = cotizacion.MontoDeDinero;
                EnvioViewModel.EsUrgente = cotizacion.EsEspecial;
                EnvioViewModel.DistanciaEntregaRecep = cotizacion.DistanciaOrigenDestino;
                EnvioViewModel.MontoTotalDelEnvio = cotizacion.MontoTotal;

                //mandar los servicios filtrados en dependencia del tipo de servicio de la cotizacion
                EnvioViewModel.Servicios = _mapper.Map<ICollection<ServicioViewModel>>(db.Servicios.Where(it => it.TipoDeServicioId == cotizacion.TipoDeServicioId)).ToList();
            }

            //sacar el Id del Cliente que esta haciendo la solicitud del envio
            var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;
            var Cliente = Utileria.GetClienteByUser(CurrentUser);

            EnvioViewModel.ClienteId = Cliente != null ? Cliente.Id : -1;

            //sino tiene credito ocultar de la lista de tipos de pagos la ópcion de creditos
            //var TieneCredito = db.Creditos.Where(it => it.FechaDeInicio <= DateTime.Now && it.FechaDeVencimiento >= DateTime.Now && it.ClienteId == EnvioViewModel.ClienteId && it.Pagos.Count <= 0).Count() > 0;
            var TieneCredito = TieneCreditoCliente(EnvioViewModel.ClienteId);

            EnvioViewModel.TieneCredito = TieneCredito;
            return View(EnvioViewModel);
        }

        // GET: Envios/Asignacion
        [Authorize(Roles = "Admin")]
        public ActionResult Asignacion(int? id)
        {
            var Envio = db.Envios.FirstOrDefault(it => it.Id == id);

            if (id == null || Envio == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var AsignacionViewModel = new AsignarMotorizadoViewModel();

            AsignacionViewModel.Envio = _mapper.Map<EnvioViewModel>(Envio);

            //Los motorizados que se pueden asignar son los que estan activos a la espera de un pedido, que no hayan sido denegados y que su proceso de afiliacion no este en solicitud apenas
            var Motorizados = db.Motorizados.Where(x => x.EstadoDelMotorizado == (short)EstadoDeMotorizadoEnum.Activo && x.EstadoDeAfiliado != (short)EstadoDeAfiliadoEnum.Denegado && x.EstadoDeAfiliado != (short)EstadoDeAfiliadoEnum.Solicitud);

            //para efectos de prueba se estara trabajando todos con los motorizados sin importar su estado
            var MotorizadosPrueba = db.Motorizados.ToList();
            AsignacionViewModel.Motorizados = _mapper.Map<ICollection<AsignacionMotorizadoViewModel>>(MotorizadosPrueba).ToList();

            return View(AsignacionViewModel);
        }

        // Post: Envios/Asignacion
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AsignacionConfirmed(int? MotorizadoId, int? EnvioId)
        {
            bool Reasignacion = false;
            Motorizado MotorizadoDelEnvio = null;

            var Motorizado = db.Motorizados.FirstOrDefault(it => it.Id == MotorizadoId);
            ///activar estas validaciones cuando este en produccion
            ////validaciones sobre el motorizado seleccionado
            //if(Motorizado==null)
            //    return Json(new { message = "El motorizado seleccionado es invalido", exito = false }, JsonRequestBehavior.AllowGet);
            //if(Motorizado.EstadoDelMotorizado != (short)EstadoDeMotorizadoEnum.Activo)
            //    return Json(new { message = "El motorizado no se encuentra disponible para realizar este envio", exito = false }, JsonRequestBehavior.AllowGet);

            var Envio = db.Envios.FirstOrDefault(it => it.Id == EnvioId);

            //validaciones sobre el envio seleccionado
            if (Envio == null || Envio.MontoTotalDelEnvio <= 0 || Envio.DistanciaEntregaRecep <= 0)
                return Json(new { message = "El envio seleccionado es invalido", exito = false }, JsonRequestBehavior.AllowGet);
            if (Envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado)
                return Json(new { message = "Este envio ya se encuentra finalizado, no se puede realizar la asignación", exito = false }, JsonRequestBehavior.AllowGet);

            //verificar si ya se encuentra asignado, en este caso seria reasignacion 
            if (Envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso && Envio.MotorizadoId > 0)
            {
                //guardar temporalmente el motorizado que estaba asignado para despues cambiarle su estado
                MotorizadoDelEnvio = db.Motorizados.FirstOrDefault(it => it.Id == Envio.MotorizadoId);

                if (MotorizadoDelEnvio != null)
                    Reasignacion = true;
            }
            else
                Reasignacion = false;

            Envio.MotorizadoId = Motorizado.Id;
            Envio.EstadoDelEnvio = (short)EstadoDelEnvioEnum.EnProceso;
            db.Entry(Envio).State = EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                if (Reasignacion)
                {
                    MotorizadoServices.CambiarEstadoMotorizado(EstadoDeMotorizadoEnum.Ocupado, Motorizado);//poner como ocupado el nuevo asignado
                    MotorizadoServices.CambiarEstadoMotorizado(EstadoDeMotorizadoEnum.Activo, MotorizadoDelEnvio);//poner como activo el viejo
                    return Json(new { message = "Se ha realizado con exito la reasignación del motorizado", exito = true }, JsonRequestBehavior.AllowGet);
                }

                //si todo esta correcto, como se esta haciendo manual la asignacion entonces manualmente debemos cambiar el estado del motorizado
                if (!MotorizadoServices.CambiarEstadoMotorizado(EstadoDeMotorizadoEnum.Ocupado, Motorizado))
                    return Json(new { message = "Se ha realizado con exito la asignación del motorizado pero no se pudo cambiar su estado a Ocupado", exito = false }, JsonRequestBehavior.AllowGet);


                return Json(new { message = "Se ha realizado con exito la asignación del motorizado", exito = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { message = "Ha ocurrido un error en la asignación del motorizado", exito = false }, JsonRequestBehavior.AllowGet);
        }

        // POST: Envios/Createss
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create([Bind(Include = "Id,DescripcionDeEnvio,FechaDelEnvio,LugarOrigen,LugarDestino,DistanciaEntregaRecep,NombresDelReceptor,CedulaDelReceptor,Peso,MontoDeDinero,TelefonoDelReceptor,EsUrgente,DebeRegresarATienda,DebeRecibirDinero,MontoARecibir,DebeRecibirCambio,MontoCambio,EstadoDelEnvio,ClienteId,TipoDePagoId,EsAlCredito,TipoDeServicioId,ServicioId,CotizacionId, Servicio")] SolicitudEnvioViewModel envio)
        {
            //capturar el valor del query string en el envioviewmodel pero que no lo valide, ya que, no son obligatorios
            ModelState.Remove("envio.Servicio.DescripcionDelServicio");
            ModelState.Remove("envio.Cotizacion.DescripcionDeCotizacion");

            if (ModelState.IsValid)
            {
                var InvalidMessage = CotizacionServices.ValidarDatosCotizacion(envio.TipoDeServicioId, envio.DistanciaEntregaRecep, envio.MontoDeDinero, envio.FechaDelEnvio);

                if (!string.IsNullOrEmpty(InvalidMessage))//si hay algun mensaje de error devolverlo
                    return Json(new { message = InvalidMessage, exito = false }, JsonRequestBehavior.AllowGet);

                var CostoAsociado = CostoServices.ValidarVigenciaCostos(envio.TipoDeServicioId, envio.FechaDelEnvio, envio.MontoDeDinero);

                if (CostoAsociado == null)//si hasta este punto sigue sin encontrarse un costo vigente asociado significa que no hay un costo para el tipo de servicio pasado como parametros
                    return Json(new { message = "No se encontró ningun costo vigente asociado al tipo de servicio seleccionado, para mayor información contactese con atención al cliente", exito = false }, JsonRequestBehavior.AllowGet);
                else
                {
                    if (User.IsInRole("Cliente"))//un envio necesita el clienteId por lo que solo este rol puede crear
                    {
                        var mEnvio = new Envio();

                        //si es un nuevo servicio guardarlo antes de ocupar servicioId
                        if (envio.ServicioId == -1)
                        {
                            if (envio.Servicio.DescripcionDelServicio != null)
                            {
                                envio.Servicio.TipoDeServicioId = envio.TipoDeServicioId;
                                envio.Servicio.Estado = true;

                                var service = _mapper.Map<Servicio>(envio.Servicio);
                                db.Servicios.Add(service);
                                db.SaveChanges();

                                //Una vez guardado el nuevo servicio actualizar la informacion del envio
                                mEnvio.ServicioId = service.Id;
                            }
                        }
                        else
                            mEnvio.ServicioId = envio.ServicioId;

                        //si el envio viene mediante una cotizacion trabajar con esa cotizacion primero, por si corrompieron la informacion en la vista, mas largo el proceso pero mas seguro
                        var cotizacion = db.Cotizaciones.FirstOrDefault(it => it.Id == envio.CotizacionId);

                        if (cotizacion != null)//si el envio es mediante cotizacion entonces ocupar los datos de la cotizacion, porque en la vista del envio los pudieron corromper
                        {
                            if (cotizacion.MontoTotal > 0)
                            {
                                if (envio.DebeRegresarATienda)
                                {
                                    var CostoA = GetCostoAsociado(envio.TipoDeServicioId, envio.FechaDelEnvio, envio.MontoDeDinero, envio.DistanciaEntregaRecep);
                                    cotizacion.MontoTotal += (decimal)CostoA.PrecioDeRegreso;
                                }
                                 
                                //datos que vienen por cotizacion
                                mEnvio.CotizacionId = cotizacion.Id;
                                mEnvio.TipoDeServicioId = cotizacion.TipoDeServicioId;
                                mEnvio.LugarOrigen = cotizacion.LugarOrigen != null ? cotizacion.LugarOrigen : _mapper.Map<Lugar>(envio.LugarOrigen);//cuando es cotizacion de gestion bancaria la cotizacio no trae un lugar origen-destino
                                mEnvio.LugarDestino = cotizacion.LugarDestino != null ? cotizacion.LugarDestino : _mapper.Map<Lugar>(envio.LugarDestino);//entonces asignarle el origen-destino seleccionado en la vista de crear envio
                                mEnvio.MontoDeDinero = cotizacion.MontoDeDinero;
                                mEnvio.EsUrgente = cotizacion.EsEspecial;
                                mEnvio.DistanciaEntregaRecep = cotizacion.DistanciaOrigenDestino > 0 ? cotizacion.DistanciaOrigenDestino : envio.DistanciaEntregaRecep;
                                mEnvio.MontoTotalDelEnvio = cotizacion.MontoTotal;

                                //si es un nuevo servicio 

                                //datos que siempre se tomaran del envio
                                mEnvio.DescripcionDeEnvio = envio.DescripcionDeEnvio;
                                mEnvio.FechaDelEnvio = envio.FechaDelEnvio;
                                mEnvio.TipoDePagoId = envio.TipoDePagoId;
                                //mEnvio.ServicioId = envio.ServicioId;
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
                                mEnvio.EsAlCredito = TieneCreditoCliente(envio.ClienteId) ? envio.EsAlCredito : false; //si el cliente tiene credito entonces lo que el envio desde la vista de lo contrario falso
                                mEnvio.CreditoId = TieneCreditoCliente(envio.ClienteId) ? GetFirstCreditoCliente(envio.ClienteId) : null;//si tiene credito entonces asociar el envio con el primer credito vigente y sin pagar
                            }
                        }
                        else
                        {
                            var MontoTotalDelEnvio = CotizacionServices.Cotizar(envio.TipoDeServicioId, envio.FechaDelEnvio, envio.MontoDeDinero, envio.DistanciaEntregaRecep, envio.EsUrgente);

                            if (MontoTotalDelEnvio > 0)
                            {
                                if (envio.DebeRegresarATienda)
                                {
                                    var CostoA = GetCostoAsociado(envio.TipoDeServicioId, envio.FechaDelEnvio, envio.MontoDeDinero, envio.DistanciaEntregaRecep);
                                    MontoTotalDelEnvio += (decimal)CostoA.PrecioDeRegreso;
                                }

                                mEnvio.DescripcionDeEnvio = envio.DescripcionDeEnvio;
                                mEnvio.FechaDelEnvio = envio.FechaDelEnvio;
                                mEnvio.TipoDePagoId = envio.TipoDeServicioId;
                                mEnvio.TipoDeServicioId = envio.TipoDeServicioId;
                                mEnvio.NombresDelReceptor = envio.NombresDelReceptor;
                                mEnvio.CedulaDelReceptor = envio.CedulaDelReceptor;
                                mEnvio.TelefonoDelReceptor = envio.TelefonoDelReceptor;
                                mEnvio.MontoDeDinero = envio.MontoDeDinero;
                                mEnvio.EsUrgente = envio.EsUrgente;
                                mEnvio.Peso = envio.Peso;
                                mEnvio.DebeRegresarATienda = envio.DebeRegresarATienda;
                                mEnvio.DebeRecibirDinero = envio.DebeRecibirDinero;
                                mEnvio.MontoARecibir = envio.MontoARecibir;
                                mEnvio.DebeRecibirCambio = envio.DebeRecibirCambio;
                                mEnvio.MontoCambio = envio.MontoCambio;
                                mEnvio.LugarOrigen = _mapper.Map<Lugar>(envio.LugarOrigen);
                                mEnvio.LugarDestino = _mapper.Map<Lugar>(envio.LugarDestino);
                                mEnvio.DistanciaEntregaRecep = envio.DistanciaEntregaRecep;
                                mEnvio.EstadoDelEnvio = envio.EstadoDelEnvio;
                                mEnvio.ClienteId = envio.ClienteId;
                                mEnvio.CotizacionId = envio.CotizacionId;
                                mEnvio.MontoTotalDelEnvio = MontoTotalDelEnvio;
                                mEnvio.EsAlCredito = TieneCreditoCliente(envio.ClienteId) ? envio.EsAlCredito : false;//si el cliente tiene credito entonces lo que el envio desde la vista de lo contrario falso
                                mEnvio.CreditoId = TieneCreditoCliente(envio.ClienteId) ? GetFirstCreditoCliente(envio.ClienteId) : null;//si tiene credito entonces asociar el envio con el primer credito vigente y sin pagar
                            }
                        }

                        //despues de asignada la informacion correspondiente
                        db.Envios.Add(mEnvio);

                        if (db.SaveChanges() > 0)
                            return Json(new { exito = true, message = "Estimado cliente se ha realizado con éxito la solicitud de su envío" }, JsonRequestBehavior.AllowGet);
                        else
                            return Json(new { exito = false, message = "No se ha podido guardar tu solicitud, para mayor información contactese con atención al cliente" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                        return Json(new { message = "Ha ocurrido un error procesando su solicitud. Solo un usuario cliente puede solicitar un envio", exito = false }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { message = "Ha ocurrido un error procesando su solicitud!", exito = false }, JsonRequestBehavior.AllowGet);
        }

        // GET: Envios/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Envio envio = db.Envios.Find(id);
            db.Envios.Remove(envio);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public ActionResult RechazarEnvio(int id, string motivo)
        {
            var envio = db.Envios.Find(id);

            if (envio != null)
            {
                if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado)
                    return Json(new { exito = false, message = "No se puede rechazar porque este envio ya se encuentra finalizado" }, JsonRequestBehavior.AllowGet); ;
                if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Rechazado)
                    return Json(new { exito = false, message = "Este envio ya se encuentra rechazado" }, JsonRequestBehavior.AllowGet);
                if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso)
                    return Json(new { exito = false, message = "No se puede rechazar porque este envio ya se encuentra en proceso" }, JsonRequestBehavior.AllowGet);

                if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Solicitud)
                {
                    envio.EstadoDelEnvio = (short)EstadoDelEnvioEnum.Rechazado;
                    envio.MotivoDeRechazo = motivo.Trim();

                    db.Entry(envio).State = EntityState.Modified;

                    if (db.SaveChanges() > 0)
                        return Json(new { exito = true, message = "Se realizo con exito el rechazo del envio" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { exito = false, message = "Tuvimos un inconveniente realizando el rechazo del envio" }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { exito = false, message = "No se pudo realizar el rechazo del envio" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult FinalizarEnvio(int id)
        {
            var envio = db.Envios.Find(id);

            if (envio != null)
            {
                if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado)
                    return Json(new { exito = false, check= envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado ? true : false, message = "Este envio ya se encuentra finalizado" }, JsonRequestBehavior.AllowGet); ;
                if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Rechazado)
                    return Json(new { exito = false, check = envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado ? true : false, message = "No se puede finalizar este envio, porque se encuentra rechazado" }, JsonRequestBehavior.AllowGet);
                if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Solicitud)
                    return Json(new { exito = false, check = envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado ? true : false, message = "No se puede finalizar este envio, porque apenas se encuentra en solicitud" }, JsonRequestBehavior.AllowGet);
                
                if (envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso)
                {
                    envio.EstadoDelEnvio = (short)EstadoDelEnvioEnum.Realizado;
                    db.Entry(envio).State = EntityState.Modified;

                    if(db.SaveChanges() > 0)
                        return Json(new { exito = true, check = envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado ? true : false, message = "Se realizo con exito la finalización del envio" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { exito = false, check = envio.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado ? true : false, message = "Tuvimos un inconveniente realizando la finalización del envio" }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { exito = false, message = "No se pudo realizar la finalización del envio" }, JsonRequestBehavior.AllowGet);
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
        public JsonResult CostoDelEnvio(int TipoDeServicioId, DateTime Fecha, float Distancia, bool Urgente, decimal MontoGestion, bool debeRegresar = false)
        {
            Fecha = DateTime.Now;
            var CostoAsociado = CostoServices.ValidarVigenciaCostos(TipoDeServicioId, Fecha, MontoGestion);

            if (CostoAsociado == null)//si hasta este punto sigue sin encontrarse un costo vigente asociado significa que no hay un costo para el tipo de servicio pasado como parametros
                return Json(new { message = "No se encontró ningun costo asociado al tipo de servicio seleccionado, para mayor información contactese con atención al cliente", exito = false }, JsonRequestBehavior.AllowGet);

            var MontoTotal = CotizacionServices.Cotizar(TipoDeServicioId, Fecha, MontoGestion, Distancia, Urgente);

            if (debeRegresar)
            {
                System.Reflection.PropertyInfo propertyInfo = CostoAsociado.GetType().GetProperty("PrecioDeRegreso");
                var PrecioDeRegreso = propertyInfo.GetValue(CostoAsociado,null);

                MontoTotal += Convert.ToDecimal(PrecioDeRegreso.ToString());
            } 

            if (MontoTotal > 0)
                return Json(new { data = MontoTotal, exito = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { message = "Ha sucedido un inconveniente al realizar tu cotización, para mayor información contactese con atención al cliente", exito = false }, JsonRequestBehavior.AllowGet);
        }

        public bool TieneCreditoCliente(int ClienteId)
        {
            var defaultCancelacionDate = DateTime.Parse("01/01/1900");
            //creditos vigentes, activos, sin pagos y sin fecha de cancelacion
            return db.Creditos.Where(it => it.FechaDeInicio <= DateTime.Now && it.FechaDeVencimiento >= DateTime.Now && it.ClienteId == ClienteId && it.Pagos.Count <= 0 && it.EstadoDelCredito && it.FechaDeCancelacion==defaultCancelacionDate ).Count() > 0;

        }

        public int? GetFirstCreditoCliente(int ClienteId)
        {
            var defaultCancelacionDate = DateTime.Parse("01/01/1900");
            //creditos vigentes, activos, sin pagos y sin fecha de cancelacion
            var creditos = db.Creditos.Where(it => it.FechaDeInicio <= DateTime.Now && it.FechaDeVencimiento >= DateTime.Now && it.ClienteId == ClienteId && it.Pagos.Count <= 0 && it.EstadoDelCredito && it.FechaDeCancelacion == defaultCancelacionDate);

            int? creditoId = null;

            if (creditos.FirstOrDefault() != null)
                creditoId = creditos.FirstOrDefault().Id;

            return creditoId;    
        }

        public Costo GetCostoAsociado(int TipoDeServicioId, DateTime FechaDeLaCotizacion, decimal MontoDeDinero, float DistanciaOrigenDestino)
        {
            var CostoAsociado = new object();

            //obtener el costo asociado al tipo de servicio pero que este activo y en vigencia.
            CostoAsociado = db.Costos.DefaultIfEmpty(null).FirstOrDefault(x => x.TipoDeServicioId == TipoDeServicioId && x.EstadoDelCosto && x.FechaDeFin > FechaDeLaCotizacion);

            if (CostoAsociado != null && MontoDeDinero <= 0 && DistanciaOrigenDestino > 0)
                return CostoAsociado as Costo;
            else
            {
                CostoAsociado = db.CostoGestionBancaria.DefaultIfEmpty(null).FirstOrDefault(x => (x.TipoDeServicioId == TipoDeServicioId && x.Estado && x.FechaDeFin > FechaDeLaCotizacion));

                if (CostoAsociado != null)
                {
                    var CostoPorcentaje = (from cb in db.CostoGestionBancaria
                                           where cb.TipoDeServicioId == TipoDeServicioId &&
                                           cb.Estado && cb.FechaDeInicio < FechaDeLaCotizacion &&
                                           cb.FechaDeFin > FechaDeLaCotizacion &&
                                           MontoDeDinero >= cb.MontoDesde &&
                                           MontoDeDinero <= cb.MontoHasta
                                           select cb);

                    CostoAsociado = CostoPorcentaje.First();
                }
            };

            return CostoAsociado as Costo;
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
