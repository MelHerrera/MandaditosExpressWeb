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
    public class CreditosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: Creditos
        public ActionResult Index()
        {
            var creditos = db.Creditos.Include(c => c.Cliente);
            return View(creditos.ToList());
        }

        // GET: Creditos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credito credito = db.Creditos.Find(id);
            if (credito == null)
            {
                return HttpNotFound();
            }
            return View(credito);
        }

        // GET: Creditos/Create
        public ActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(db.Clientes, "Id", "NombreCompleto");
            return View();
        }

        // POST: Creditos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FechaDeInicio,FechaDeVencimiento,EstadoDelCredito,FechaDeCancelacion,ClienteId")] Credito credito)
        {
            if (ModelState.IsValid)
            {
                db.Creditos.Add(credito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", credito.ClienteId);
            return View(credito);
        }

        // POST: Creditos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FechaDeInicio,FechaDeVencimiento,EstadoDelCredito,FechaDeCancelacion,ClienteId")] Credito credito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(credito).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { exito = true }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", credito.ClienteId);
            return Json(new { exito = false }, JsonRequestBehavior.AllowGet);
        }


        // POST: Creditos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Credito credito)
        {
            Credito Ocredito = db.Creditos.Find(credito.Id);
            db.Creditos.Remove(Ocredito);

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
