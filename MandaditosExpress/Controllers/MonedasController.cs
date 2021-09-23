using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MandaditosExpress.Models;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize]
    public class MonedasController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: Monedas
        public ActionResult Index()
        {
            return View(db.Monedas.ToList());
        }

        // GET: Monedas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moneda moneda = db.Monedas.Find(id);
            if (moneda == null)
            {
                return HttpNotFound();
            }
            return View(moneda);
        }

        // GET: Monedas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Monedas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NombreDeMoneda,Abreviatura,EstadoMoneda")] Moneda moneda)
        {
            if (ModelState.IsValid)
            {
                moneda.Estado = true;
                db.Monedas.Add(moneda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(moneda);
        }

        //// GET: Monedas/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Moneda moneda = db.Monedas.Find(id);
        //    if (moneda == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(moneda);
        //}

        // POST: Monedas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Moneda moneda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moneda).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { exito = true },JsonRequestBehavior.AllowGet);
            }
            return Json(new { exito = false },JsonRequestBehavior.AllowGet);
        }

        // GET: Monedas/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Moneda moneda = db.Monedas.Find(id);
        //    if (moneda == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(moneda);
        //}

        // POST: Monedas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Moneda moneda)
        {
            Moneda Omoneda = db.Monedas.Find(moneda.Id);
            db.Monedas.Remove(Omoneda);
            if (db.SaveChanges() > 0)
            {
                return Json(new { exito = true }, JsonRequestBehavior.AllowGet);
            }

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
