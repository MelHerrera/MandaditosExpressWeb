﻿using System;
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
    public class CostosGestionBancariaController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: CostosGestionBancaria
        public ActionResult Index()
        {
            var data = db.CostoGestionBancaria.ToList();
            return View(data);
        }

        // GET: CostosGestionBancaria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostoGestionBancaria costoGestionBancaria = db.CostoGestionBancaria.Find(id);
            if (costoGestionBancaria == null)
            {
                return HttpNotFound();
            }
            return View(costoGestionBancaria);
        }

        // GET: CostosGestionBancaria/Create
        public ActionResult Create()
        {
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio.Where(tp => tp.DescripcionTipoDeServicio.ToUpper().Contains("BANC")), "Id", "DescripcionTipoDeServicio");
            return View();
        }

        // POST: CostosGestionBancaria/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FechaDeInicio,FechaDeFin,Descripcion,MontoDesde,MontoHasta,Estado,Porcentaje,PrecioDeRecargo,TipoDeServicioId")] CostoGestionBancaria costoGestionBancaria)
        {
            if (ModelState.IsValid)
            {
                db.CostoGestionBancaria.Add(costoGestionBancaria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoDeServicio = new SelectList(db.TiposDeServicio, "Id", "Descripcion", costoGestionBancaria.TipoDeServicioId);
            return View(costoGestionBancaria);
        }

        // POST: CostosGestionBancaria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FechaDeInicio,FechaDeFin,Descripcion,MontoDesde,MontoHasta,Estado,Porcentaje,PrecioDeRecargo,TipoDeServicioId")] CostoGestionBancaria costoGestionBancaria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costoGestionBancaria).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { exito = true }, JsonRequestBehavior.AllowGet);
            }

            ViewBag.TipoDeServicio = new SelectList(db.TiposDeServicio, "Id", "Descripcion", costoGestionBancaria.TipoDeServicioId);
            return Json(new { exito = false }, JsonRequestBehavior.AllowGet);
        }

        // POST: Creditos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(CostoGestionBancaria costoGestionBancaria)
        {
            CostoGestionBancaria xcosto = db.CostoGestionBancaria.Find(costoGestionBancaria.Id);
            db.CostoGestionBancaria.Remove(xcosto);

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
