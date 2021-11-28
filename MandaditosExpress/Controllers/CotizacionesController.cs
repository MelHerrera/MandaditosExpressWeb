using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.ViewModels;
using MandaditosExpress.Services;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin, Cliente")]
    public class CotizacionesController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private CostoServices CostoServices;
        private CotizacionServices CotizacionServices;
        private readonly IMapper _mapper;
        public CotizacionesController(IMapper mapper)
        {
            CostoServices = new CostoServices(db);
            CotizacionServices = new CotizacionServices(db);
            _mapper = mapper;
        }

        // GET: Cotizaciones
        public ActionResult Index()
        {
            var cotizaciones = db.Cotizaciones.Include(c => c.Cliente).Include(c => c.TipoDeServicio).Where(x => x.FechaDeValidez >= DateTime.Now); ;

            //Validacion por si ya viene una cotizacion desde la autenticacion
            var cotizacion = TempData.ContainsKey("Cotizacion") ? (CotizacionViewModel)TempData["Cotizacion"] : null;

            //guardar una cotizacion requiere el clienteId de esa cotizacion, por lo que es el unico rol que puede guardar cotizaciones
            if (User.IsInRole("Cliente"))
            {
                if (cotizacion != null)
                {
                    var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;

                    var mCotiza = new Cotizacion
                    {
                        DescripcionDeCotizacion = cotizacion.DescripcionDeCotizacion,
                        FechaDeLaCotizacion = cotizacion.FechaDeLaCotizacion,
                        FechaDeValidez = cotizacion.FechaDeValidez,
                        LugarOrigen = _mapper.Map<Lugar>(cotizacion.LugarOrigen),
                        LugarDestino = _mapper.Map<Lugar>(cotizacion.LugarDestino),
                        DistanciaOrigenDestino = cotizacion.DistanciaOrigenDestino,
                        EsEspecial = cotizacion.EsEspecial,
                        MontoTotal = cotizacion.MontoTotal,
                        ClienteId = cotizacion.ClienteId > 0 ? cotizacion.ClienteId : GetCurrentCliente(CurrentUser) != null ? GetCurrentCliente(CurrentUser).Id : -1,
                        TipoDeServicioId = cotizacion.TipoDeServicioId,
                        MontoDeDinero = cotizacion.MontoDeDinero
                    };

                    db.Cotizaciones.Add(mCotiza);
                    db.SaveChanges();
                }
            }


            return View(cotizaciones.ToList());
        }

        // GET: Cotizaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cotizacion cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }
            return View(cotizacion);
        }

        // GET: Cotizaciones/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            var mCotizacionViewModel = new CotizacionViewModel();

            var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;
            var CurrentCliente = GetCurrentCliente(CurrentUser);

            ViewBag.Cliente = CurrentCliente != null ? CurrentCliente.PrimerNombre : "";

            mCotizacionViewModel.ClienteId = CurrentCliente != null ? CurrentCliente.Id : -1;

            return View(mCotizacionViewModel);
        }

        // POST: Cotizaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create(CotizacionViewModel cotizacion)
        {
            try
            {
                var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;
                var CurrentCliente = db.Clientes.FirstOrDefault(c => c.CorreoElectronico == CurrentUser);

                ViewBag.Cliente = CurrentCliente != null ? CurrentCliente.PrimerNombre : "";
                ViewBag.ClienteId = CurrentCliente != null ? CurrentCliente.Id : -1;
                ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio, "Id", "DescripcionTipoDeServicio");

                var InvalidMessage = CotizacionServices.ValidarDatosCotizacion(cotizacion.TipoDeServicioId, cotizacion.DistanciaOrigenDestino, cotizacion.MontoDeDinero, cotizacion.FechaDeLaCotizacion);

                if (!string.IsNullOrEmpty(InvalidMessage))//si hay algun mensaje de error devolverlo
                    return Json(new { message = InvalidMessage, exito = false }, JsonRequestBehavior.AllowGet);

                var ValidarVigenciaCostoAsociado = CostoServices.ValidarVigenciaCostos(cotizacion.TipoDeServicioId, cotizacion.FechaDeLaCotizacion, cotizacion.MontoDeDinero);

                if (ValidarVigenciaCostoAsociado == null)//si hasta este punto sigue sin encontrarse un costo vigente asociado significa que no hay un costo para el tipo de servicio pasado como parametros
                    return Json(new { message = "No se encontró ningun costo vigente asociado al tipo de servicio seleccionado, para mayor información contactese con atención al cliente", exito = false }, JsonRequestBehavior.AllowGet);

                if (ModelState.IsValid)
                {
                    var CostoTotal = 0.0M;
                    //obtener el costo asociado al tipo de servicio pero que este activo y en vigencia.
                    var CostoAsociado = db.Costos.DefaultIfEmpty(null).FirstOrDefault(x => (x.TipoDeServicioId == cotizacion.TipoDeServicioId && x.EstadoDelCosto && x.FechaDeFin > cotizacion.FechaDeLaCotizacion));

                    if (CostoAsociado != null && cotizacion.MontoDeDinero <= 0)
                    {
                        if (cotizacion.DistanciaOrigenDestino > 0)
                        {
                            CostoTotal = (decimal)(CostoAsociado.CostoDeAsistencia + CostoAsociado.CostoDeGasolina + CostoAsociado.CostoDeMotorizado +
                                               ((CostoAsociado.DistanciaBase + cotizacion.DistanciaOrigenDestino) * CostoAsociado.PrecioPorKm));

                            if (cotizacion.EsEspecial)
                                CostoTotal += (decimal)CostoAsociado.PrecioDeRecargo;
                        }
                        else
                            return Json(new { message = "La distancia a cotizar debe ser mayor a 0", exito = false }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (cotizacion.MontoDeDinero >= Utilidades.MinGestionBancaria && cotizacion.MontoDeDinero <= Utilidades.MaxGestionBancaria)//el negocio actualmente solo realiza gestiones bancarias en este rango
                        {
                            var CostoGestion = db.CostoGestionBancaria.DefaultIfEmpty(null).FirstOrDefault(x => (x.TipoDeServicioId == cotizacion.TipoDeServicioId && x.Estado && x.FechaDeFin > cotizacion.FechaDeLaCotizacion));

                            if (CostoGestion != null)
                            {
                                var CostoPorcentaje = (from cb in db.CostoGestionBancaria
                                                       where cb.TipoDeServicioId == cotizacion.TipoDeServicioId &&
                                                       cb.Estado && cb.FechaDeInicio < cotizacion.FechaDeLaCotizacion &&
                                                       cb.FechaDeFin > cotizacion.FechaDeLaCotizacion &&
                                                       cotizacion.MontoDeDinero >= cb.MontoDesde &&
                                                       cotizacion.MontoDeDinero <= cb.MontoHasta
                                                       select cb);

                                //se cobra un porcentaje o un valor directamente si asi se definio en el costo
                                var Porcentaje = CostoPorcentaje.Count() > 0 ? CostoPorcentaje.First().Porcentaje : 0;

                                if (Porcentaje > 0 && cotizacion.MontoDeDinero > 0)
                                    CostoTotal = cotizacion.MontoDeDinero * ((decimal)(Porcentaje / 100));
                                else
                                {
                                   var valor = CostoPorcentaje.Count() > 0 ? CostoPorcentaje.First().Valor : 0;

                                    if (valor > 0 && cotizacion.MontoDeDinero > 0)
                                        CostoTotal = valor;
                                }

                                if (cotizacion.EsEspecial)
                                    CostoTotal += (decimal)(CostoPorcentaje.Count() > 0 ? CostoPorcentaje.First().PrecioDeRecargo : 0.0f);
                            }
                        }
                        else
                            return Json(new { message = string.Format("Actualmente el negocio solo realiza gestiones bancarias con montos de {0} a {1}, para una cantidad diferente contactese con atención al cliente", Utilidades.MinGestionBancaria, Utilidades.MaxGestionBancaria), exito = false }, JsonRequestBehavior.AllowGet);

                    };

                    cotizacion.MontoTotal = CostoTotal;

                    return Json(new { exito = true, data = cotizacion });
                }
                else
                {
                    var ModelErrors = ModelState.Values.SelectMany(x => x.Errors).ToList().Select(y => y.ErrorMessage);
                    return Json(new { exito = false, data = cotizacion, havemodelerror = true, error = ModelErrors });
                }
            }
            catch (Exception)
            {
                return Json(new { message = "Ha ocurrido un error procesando su solicitud", exito = false }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Cotizaciones/Guardar
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Guardar(CotizacionViewModel cotizacion)
        {
            if (Request.IsAuthenticated )//una cotizacion necesita de un clienteId por lo que solo el rol cliente puede guardar
            {
                if (User.IsInRole("Cliente"))
                {
                    if (ModelState.IsValid)
                    {

                        var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;
                        var mCotiza = new Cotizacion();

                        if (cotizacion.DistanciaOrigenDestino > 0)
                        {
                            mCotiza.LugarOrigen = _mapper.Map<Lugar>(cotizacion.LugarOrigen);
                            mCotiza.LugarDestino = _mapper.Map<Lugar>(cotizacion.LugarDestino);
                        }

                        mCotiza = new Cotizacion
                        {
                            DescripcionDeCotizacion = cotizacion.DescripcionDeCotizacion,
                            FechaDeLaCotizacion = cotizacion.FechaDeLaCotizacion,
                            FechaDeValidez = cotizacion.FechaDeValidez,
                            DistanciaOrigenDestino = cotizacion.DistanciaOrigenDestino,
                            EsEspecial = cotizacion.EsEspecial,
                            MontoTotal = cotizacion.MontoTotal,
                            ClienteId = cotizacion.ClienteId > 0 ? cotizacion.ClienteId : GetCurrentCliente(CurrentUser) != null ? GetCurrentCliente(CurrentUser).Id : -1,
                            TipoDeServicioId = cotizacion.TipoDeServicioId,
                            MontoDeDinero = cotizacion.MontoDeDinero
                        };

                        db.Cotizaciones.Add(mCotiza);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                    ModelState.AddModelError("", "No se puede guardar una cotización como Administrador");
            }
            else
            {
                //validacion para que despues de que se autentique lo regrese a esta accion con los datos de la cotizacion
                TempData["Cotizacion"] = cotizacion;
                return RedirectToAction("Login", "Account", new { ReturnUrl = "/Cotizaciones/Index" });
            }
            return View("Create",cotizacion);
        }

        // GET: Cotizaciones/RealizarEnvio
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        public ActionResult RealizarEnvioConfirmed()
        {
            if (Request.IsAuthenticated && User.IsInRole("Cliente"))
            {
                //Validacion por si ya viene una cotizacion desde la autenticacion
                var cotizacion = TempData.ContainsKey("Cotizacion") ? (CotizacionViewModel)TempData["Cotizacion"] : null;

                if (cotizacion != null)
                {
                    var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;

                    var mCotiza = new Cotizacion
                    {
                        DescripcionDeCotizacion = cotizacion.DescripcionDeCotizacion,
                        FechaDeLaCotizacion = cotizacion.FechaDeLaCotizacion,
                        FechaDeValidez = cotizacion.FechaDeValidez,
                        LugarOrigen = _mapper.Map<Lugar>(cotizacion.LugarOrigen),
                        LugarDestino = _mapper.Map<Lugar>(cotizacion.LugarDestino),
                        DistanciaOrigenDestino = cotizacion.DistanciaOrigenDestino,
                        EsEspecial = cotizacion.EsEspecial,
                        MontoTotal = cotizacion.MontoTotal,
                        ClienteId = cotizacion.ClienteId > 0 ? cotizacion.ClienteId : GetCurrentCliente(CurrentUser) != null ? GetCurrentCliente(CurrentUser).Id : -1,
                        TipoDeServicioId = cotizacion.TipoDeServicioId,
                        MontoDeDinero = cotizacion.MontoDeDinero
                    };

                    db.Cotizaciones.Add(mCotiza);
                    db.SaveChanges();
                    return RedirectToAction("Create", "Envios", new { CotizacionId = mCotiza.Id });
                    //return Json(new { exito = true, data = mCotiza.Id }, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(new { exito = false, message = "Ha ocurrido un error. Solo clientes pueden realizar la solicitud de envios!" }, JsonRequestBehavior.AllowGet);

            return Json(new { exito = false, message = "Ha ocurrido un error procesando la solicitud del envio!" }, JsonRequestBehavior.AllowGet);
        }

        // POST: Cotizaciones/Guardar
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult RealizarEnvio(CotizacionViewModel cotizacion)
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Cliente"))
                {
                    if (ModelState.IsValid)
                    {
                        var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;

                        var mCotiza = new Cotizacion
                        {
                            DescripcionDeCotizacion = cotizacion.DescripcionDeCotizacion,
                            FechaDeLaCotizacion = cotizacion.FechaDeLaCotizacion,
                            FechaDeValidez = cotizacion.FechaDeValidez,
                            LugarOrigen = _mapper.Map<Lugar>(cotizacion.LugarOrigen),
                            LugarDestino = _mapper.Map<Lugar>(cotizacion.LugarDestino),
                            DistanciaOrigenDestino = cotizacion.DistanciaOrigenDestino,
                            EsEspecial = cotizacion.EsEspecial,
                            MontoTotal = cotizacion.MontoTotal,
                            ClienteId = cotizacion.ClienteId > 0 ? cotizacion.ClienteId : GetCurrentCliente(CurrentUser) != null ? GetCurrentCliente(CurrentUser).Id : -1,
                            TipoDeServicioId = cotizacion.TipoDeServicioId,
                            MontoDeDinero = cotizacion.MontoDeDinero
                        };

                        db.Cotizaciones.Add(mCotiza);
                        db.SaveChanges();
                        return Json(new { exito = true, data = mCotiza.Id }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(new { exito = false, message = "Ha ocurrido un error. Solo clientes pueden realizar la solicitud de envios!" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                //validacion para que despues de que se autentique lo regrese a esta accion con los datos de la cotizacion
                TempData["Cotizacion"] = cotizacion;
                return Json(new { exito = false, data = "/Cotizaciones/RealizarEnvioConfirmed" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { exito = false, message = "Ha ocurrido un error procesando la solicitud del envio!" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Cotizaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cotizacion cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", cotizacion.ClienteId);
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio, "Id", "DescripcionTipoDeServicio", cotizacion.TipoDeServicioId);
            return View(cotizacion);
        }

        // POST: Cotizaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DescripcionDeCotizacion,FechaDeLaCotizacion,FechaDeValidez,DireccionDeEntrega,DireccionDeRecepcion,DistanciaEntregaRecep,MontoDeDinero,EsEspecial,PrecioDeRecargo,GestionId,MontoTotal,ClienteId,ServicioId,TipoDeServicioId")] Cotizacion cotizacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cotizacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", cotizacion.ClienteId);
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio, "Id", "DescripcionTipoDeServicio", cotizacion.TipoDeServicioId);
            return View(cotizacion);
        }

        // GET: Cotizaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cotizacion cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }
            return View(cotizacion);
        }

        // POST: Cotizaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cotizacion cotizacion = db.Cotizaciones.Find(id);
            db.Cotizaciones.Remove(cotizacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public Cliente GetCurrentCliente(string CurrentUser)
        {
            return (CurrentUser != null && CurrentUser.Length > 0) ? db.Clientes.FirstOrDefault(c => c.CorreoElectronico == CurrentUser) : new Cliente();
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
