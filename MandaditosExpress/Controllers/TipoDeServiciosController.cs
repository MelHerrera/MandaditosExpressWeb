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
    [Authorize]
    public class TipoDeServiciosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: TipoDeServicios
        public ActionResult Index()
        {
            return View(db.TiposDeServicio.ToList());
        }

        // GET: TipoDeServicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeServicio tipoDeServicio = db.TiposDeServicio.Find(id);
            if (tipoDeServicio == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeServicio);
        }

        // GET: TipoDeServicios/Create
        public ActionResult Create()
        {
            return View(new TipoDeServicio());
        }

        // POST: TipoDeServicios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DescripcionTipoDeServicio,EstadoTipoDeServicio")] TipoDeServicio tipoDeServicio)
        {
            if (ModelState.IsValid)
            {
                db.TiposDeServicio.Add(tipoDeServicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDeServicio);
        }

        // GET: TipoDeServicios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeServicio tipoDeServicio = db.TiposDeServicio.Find(id);
            if (tipoDeServicio == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeServicio);
        }

        // POST: TipoDeServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DescripcionTipoDeServicio,EstadoTipoDeServicio")] TipoDeServicio tipoDeServicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDeServicio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDeServicio);
        }

        // GET: TipoDeServicios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeServicio tipoDeServicio = db.TiposDeServicio.Find(id);
            if (tipoDeServicio == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeServicio);
        }

        // POST: TipoDeServicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDeServicio tipoDeServicio = db.TiposDeServicio.Find(id);
            db.TiposDeServicio.Remove(tipoDeServicio);
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
