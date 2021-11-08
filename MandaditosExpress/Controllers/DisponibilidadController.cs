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
    [Authorize(Roles = "Admin")]
    public class DisponibilidadController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: Disponibilidad
        public ActionResult Index()
        {
            return View(db.Disponibilidad.ToList());
        }

        // GET: Disponibilidad/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disponibilidad disponibilidad = db.Disponibilidad.Find(id);
            if (disponibilidad == null)
            {
                return HttpNotFound();
            }
            return View(disponibilidad);
        }

        // GET: Disponibilidad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Disponibilidad/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,EstadoDeLaDisponibilidad")] Disponibilidad disponibilidad)
        {
            if (ModelState.IsValid)
            {
                db.Disponibilidad.Add(disponibilidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(disponibilidad);
        }

        // POST: Disponibilidad/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,EstadoDeLaDisponibilidad")] Disponibilidad disponibilidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disponibilidad).State = EntityState.Modified;

                if (db.SaveChanges() > 0)
                    return Json(new { exito = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { exito = false }, JsonRequestBehavior.AllowGet);
        }

        // POST: Disponibilidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Disponibilidad disponibilidad)
        {
            Disponibilidad xdisponibilidad = db.Disponibilidad.Find(disponibilidad.Id);
            db.Disponibilidad.Remove(xdisponibilidad);

            if (db.SaveChanges() > 0)
                return Json(new { exito = true }, JsonRequestBehavior.AllowGet);

            return Json(new { exito = false }, JsonRequestBehavior.AllowGet);
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
