using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MandaditosExpress.Models;
using MandaditosExpress.Models.ViewModels;

namespace MandaditosExpress.Controllers
{
    public class pruebasController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: pruebas
        public ActionResult Index()
        {
            var cotizaciones = db.Cotizaciones.Include(c => c.Cliente).Include(c => c.TipoDeServicio);
            return View(cotizaciones.ToList());
        }

        // GET: pruebas/Details/5
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

        // GET: pruebas/Create
        public ActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico");
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio, "Id", "DescripcionTipoDeServicio");

            var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;
            var CurrentCliente = db.Clientes.FirstOrDefault(c => c.CorreoElectronico == CurrentUser);

            ViewBag.Cliente = CurrentCliente != null ? CurrentCliente.PrimerNombre : "";

            var mCotizacionViewModel = new CotizacionViewModel();
            mCotizacionViewModel.ClienteId = CurrentCliente != null ? CurrentCliente.Id : -1;

            return View(mCotizacionViewModel);
        }

        // POST: pruebas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DescripcionDeCotizacion,FechaDeLaCotizacion,FechaDeValidez,DireccionDeOrigen,DireccionDestino,DistanciaOrigenDestino,EsEspecial,MontoTotal,ClienteId,TipoDeServicioId,MontoDeDinero")] CotizacionViewModel cotizacion)
        {
            if (ModelState.IsValid)
            {
                //db.Cotizaciones.Add((CotizacionViewModel)cotizacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", cotizacion.ClienteId);
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio, "Id", "DescripcionTipoDeServicio", cotizacion.TipoDeServicioId);
            return View(cotizacion);
        }

        // GET: pruebas/Edit/5
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

        // POST: pruebas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DescripcionDeCotizacion,FechaDeLaCotizacion,FechaDeValidez,DireccionDeOrigen,DireccionDestino,DistanciaOrigenDestino,EsEspecial,MontoTotal,ClienteId,TipoDeServicioId,MontoDeDinero")] Cotizacion cotizacion)
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

        // GET: pruebas/Delete/5
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

        // POST: pruebas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cotizacion cotizacion = db.Cotizaciones.Find(id);
            db.Cotizaciones.Remove(cotizacion);
            db.SaveChanges();
            return RedirectToAction("Index");
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
