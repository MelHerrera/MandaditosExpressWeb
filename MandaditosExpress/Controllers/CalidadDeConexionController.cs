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
    [Authorize(Roles = "Admin, Asistente")]
    public class CalidadDeConexionController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: VelocidadDeConexion
        public ActionResult Index()
        {
            return View(db.VelocidadDeConexion.ToList());
        }

        // GET: VelocidadDeConexion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalidadDeConexion velocidadDeConexion = db.VelocidadDeConexion.Find(id);
            if (velocidadDeConexion == null)
            {
                return HttpNotFound();
            }
            return View(velocidadDeConexion);
        }

        // GET: VelocidadDeConexion/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: VelocidadDeConexion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado")] CalidadDeConexion velocidadDeConexion)
        {
            if (ModelState.IsValid)
            {
                db.VelocidadDeConexion.Add(velocidadDeConexion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(velocidadDeConexion);
        }

        // POST: VelocidadDeConexion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado")] CalidadDeConexion velocidadDeConexion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(velocidadDeConexion).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { exito = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { exito = false }, JsonRequestBehavior.AllowGet);
        }

        // POST: VelocidadDeConexion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(CalidadDeConexion velocidadDeConexion)
        {
            CalidadDeConexion OvelocidadDeConexion = db.VelocidadDeConexion.Find(velocidadDeConexion.Id);

            if(OvelocidadDeConexion.Motorizados.Count > 0)
                return Json(new { exito = false, message = "No se puede eliminar porque tiene registros de afiliación de motorizado asociados" }, JsonRequestBehavior.AllowGet);

            db.VelocidadDeConexion.Remove(OvelocidadDeConexion);

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
