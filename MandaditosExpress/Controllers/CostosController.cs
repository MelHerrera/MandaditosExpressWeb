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
    public class CostosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: Costos
        public ActionResult Index()
        {
            return View(db.Costos.ToList());
        }

        // GET: Costos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Costo costo = db.Costos.Find(id);
            if (costo == null)
            {
                return HttpNotFound();
            }
            return View(costo);
        }

        // GET: Costos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Costos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FechaDeInicio,FechaDeFin,CostoDeGasolina,CostoDeAsistencia,CostoDeMotorizado,DistanciaBase,PrecioPorKm,EstadoDelCosto,PrecioBaseGestionBancaria,PorcentajeBaseGestionBancaria")] Costo costo)
        {
            if (ModelState.IsValid)
            {
                //activar el costo 
                costo.EstadoDelCosto = true;

                db.Costos.Add(costo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(costo);
        }

        // //GET: Costos/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Costo costo = db.Costos.Find(id);
        //    if (costo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(costo);
        //}

        // POST: Costos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FechaDeInicio,FechaDeFin,CostoDeGasolina,CostoDeAsistencia,CostoDeMotorizado,DistanciaBase,PrecioPorKm,EstadoDelCosto,PrecioBaseGestionBancaria,PorcentajeBaseGestionBancaria")] Costo costo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costo).State = EntityState.Modified;
                db.SaveChanges();
                return View();
                //var redirectUrl = new UrlHelper().Action("Index", "Costos");
                //return Json(new { Url = redirectUrl });
            }
            return View(costo);
        }

        // GET: Costos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Costo costo = db.Costos.Find(id);
            if (costo == null)
            {
                return HttpNotFound();
            }
            return View(costo);
        }

        // POST: Costos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Costo costo = db.Costos.Find(id);
            db.Costos.Remove(costo);
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
