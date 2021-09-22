using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MandaditosExpress.Models;
using MandaditosExpress.Models.ViewModels;

namespace MandaditosExpress.Controllers
{
    [Authorize]
    public class CotizacionesController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: Cotizaciones
        public ActionResult Index()
        {
            var cotizaciones = db.Cotizaciones.Include(c => c.Cliente).Include(c => c.TipoDeServicio);
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

            //Validacion por si ya viene una cotizacion desde la autenticacion
            var cotizacion = TempData.ContainsKey("Cotizacion") ? TempData["Cotizacion"] : null;
            if (cotizacion != null)
                mCotizacionViewModel =(CotizacionViewModel) cotizacion;

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
            var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;
            var CurrentCliente = db.Clientes.FirstOrDefault(c => c.CorreoElectronico == CurrentUser);

            ViewBag.Cliente = CurrentCliente != null ? CurrentCliente.PrimerNombre : "";
            ViewBag.ClienteId = CurrentCliente != null ? CurrentCliente.Id : -1;
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio, "Id", "DescripcionTipoDeServicio");

            if (ModelState.IsValid)
            {
                var CostoTotal = 0.0M;
                //obtener el costo asociado al tipo de servicio pero que este activo y en vigencia.
                var CostoAsociado = db.Costos.DefaultIfEmpty(null).FirstOrDefault(x => (x.TipoDeServicioId == cotizacion.TipoDeServicioId && x.EstadoDelCosto && x.FechaDeFin > cotizacion.FechaDeLaCotizacion));

                if (CostoAsociado != null && cotizacion.MontoDeDinero <= 0 && cotizacion.DistanciaOrigenDestino > 0)
                {
                    CostoTotal = (decimal)(CostoAsociado.CostoDeAsistencia + CostoAsociado.CostoDeGasolina + CostoAsociado.CostoDeMotorizado +
                    ((CostoAsociado.DistanciaBase + cotizacion.DistanciaOrigenDestino) * CostoAsociado.PrecioPorKm));

                    if (cotizacion.EsEspecial)
                        CostoTotal += (decimal)CostoAsociado.PrecioDeRecargo;
                }
                else
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
                        var Porcentaje = CostoPorcentaje.Count() > 0 ? CostoPorcentaje.First().Porcentaje : 0;

                        if (Porcentaje > 0 && cotizacion.MontoDeDinero > 0)
                            CostoTotal = cotizacion.MontoDeDinero * ((decimal)(Porcentaje / 100));

                        if (cotizacion.EsEspecial)
                            CostoTotal += (decimal)CostoGestion.PrecioDeRecargo;
                    }
                };

                cotizacion.MontoTotal = CostoTotal;

                return Json(new { exito=true, data=cotizacion});
            }

            return Json(new { exito = false, data = cotizacion });
        }

        // POST: Cotizaciones/Guardar
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Guardar(CotizacionViewModel cotizacion)
        {
            if (Request.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var mCotiza = new Cotizacion {
                        DescripcionDeCotizacion=cotizacion.DescripcionDeCotizacion, 
                        FechaDeLaCotizacion= cotizacion.FechaDeLaCotizacion ,
                        FechaDeValidez= cotizacion.FechaDeValidez,
                        LugarOrigen=cotizacion.LugarDeOrigen,
                        LugarDestino=cotizacion.LugarDestino,
                        DistanciaOrigenDestino= cotizacion.DistanciaOrigenDestino,
                        EsEspecial= cotizacion.EsEspecial,
                        MontoTotal= cotizacion.MontoTotal,
                        ClienteId= cotizacion.ClienteId,
                        TipoDeServicioId= cotizacion.TipoDeServicioId,
                        MontoDeDinero= cotizacion.MontoDeDinero
                    };

                    var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;
                    var CurrentCliente = GetCurrentCliente(CurrentUser);

                    mCotiza.ClienteId = CurrentCliente != null ? CurrentCliente.Id : -1;

                    db.Cotizaciones.Add(mCotiza);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                //validacion para que despues de que se autentique lo regrese a esta accion con los datos de la cotizacion
                TempData["Cotizacion"] = cotizacion;
                return RedirectToAction("Login", "Account",new { ReturnUrl = "/Cotizaciones/Create" });
            }
                

            return View(cotizacion);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Save(CotizacionViewModel cotizacion)
        {
            return Json("Succesfull");
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
