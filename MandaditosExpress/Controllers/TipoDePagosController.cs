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
    public class TipoDePagosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: TipoDePagos
        public ActionResult Index()
        {
            return View(db.TiposDePago.ToList());
        }

        // GET: TipoDePagos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDePago tipoDePago = db.TiposDePago.Find(id);
            if (tipoDePago == null)
            {
                return HttpNotFound();
            }
            return View(tipoDePago);
        }

        // GET: TipoDePagos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDePagos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,EstadoTipoDePago")] TipoDePago tipoDePago)
        {
            if (ModelState.IsValid)
            {
                db.TiposDePago.Add(tipoDePago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDePago);
        }

        // GET: TipoDePagos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDePago tipoDePago = db.TiposDePago.Find(id);
            if (tipoDePago == null)
            {
                return HttpNotFound();
            }
            return View(tipoDePago);
        }

        // POST: TipoDePagos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,EstadoTipoDePago")] TipoDePago tipoDePago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDePago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDePago);
        }

        // GET: TipoDePagos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDePago tipoDePago = db.TiposDePago.Find(id);
            if (tipoDePago == null)
            {
                return HttpNotFound();
            }
            return View(tipoDePago);
        }

        // POST: TipoDePagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDePago tipoDePago = db.TiposDePago.Find(id);
            db.TiposDePago.Remove(tipoDePago);
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
