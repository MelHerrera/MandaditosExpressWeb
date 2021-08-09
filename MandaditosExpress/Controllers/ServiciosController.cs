using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MandaditosExpress.Models;

namespace MandaditosExpress.Controllers
{
    public class ServiciosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: Servicios
        public ActionResult Index()
        {
            var gestiones = db.Gestiones.Include(s => s.Costo).Include(s => s.TipoDeServicio);
            return View(gestiones.ToList());
        }

        // GET: Servicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servicio servicio = db.Gestiones.Find(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        // GET: Servicios/Create
        public ActionResult Create()
        {
            ViewBag.CostoId = new SelectList(db.Costos, "Id", "Id");
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio, "Id", "DescripcionTipoDeServicio");
            return View();
        }

        // POST: Servicios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DescripcionDelServicio,TipoDeServicioId,MontoTotalDelServicio,CostoId")] Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                db.Gestiones.Add(servicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CostoId = new SelectList(db.Costos, "Id", "Id", servicio.CostoId);
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio, "Id", "DescripcionTipoDeServicio", servicio.TipoDeServicioId);
            return View(servicio);
        }

        // GET: Servicios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servicio servicio = db.Gestiones.Find(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            ViewBag.CostoId = new SelectList(db.Costos, "Id", "Id", servicio.CostoId);
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio, "Id", "DescripcionTipoDeServicio", servicio.TipoDeServicioId);
            return View(servicio);
        }

        // POST: Servicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DescripcionDelServicio,TipoDeServicioId,MontoTotalDelServicio,CostoId")] Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CostoId = new SelectList(db.Costos, "Id", "Id", servicio.CostoId);
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio, "Id", "DescripcionTipoDeServicio", servicio.TipoDeServicioId);
            return View(servicio);
        }

        // GET: Servicios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servicio servicio = db.Gestiones.Find(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        // POST: Servicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Servicio servicio = db.Gestiones.Find(id);
            db.Gestiones.Remove(servicio);
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
