﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.ViewModels;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin, Asistente")]
    public class ServiciosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private IMapper _mapper;

        public ServiciosController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: Servicios
        public ActionResult Index()
        {
            var data = _mapper.Map<ICollection<ServicioViewModel>>(db.Servicios.ToList());
            return View(data.ToList());
        }

        // GET: Servicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servicio servicio = db.Servicios.Find(id);
            if (servicio == null)
            {
                return HttpNotFound();
            }
            return View(servicio);
        }

        // GET: Servicios/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio.Where(x => x.EstadoTipoDeServicio), "Id", "DescripcionTipoDeServicio");
            return View();
        }

        // POST: Servicios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "DescripcionDelServicio,TipoDeServicioId,Estado")] Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                db.Servicios.Add(servicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio.Where(x => x.EstadoTipoDeServicio), "Id", "DescripcionTipoDeServicio");
            return View(servicio);
        }

        // POST: Servicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,DescripcionDelServicio,Estado,TipoDeServicioId")] Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicio).State = EntityState.Modified;

                if (db.SaveChanges() > 0)
                    return Json(new { exito = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { exito = false }, JsonRequestBehavior.AllowGet);
        }

        // POST: Servicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(Servicio servicio)
        {
            Servicio xservicio = db.Servicios.Find(servicio.Id);

            if(xservicio.Envios.Count > 0)
                return Json(new { exito = false, message = "no se puede eliminar porque existen envios asociados a este servicio" }, JsonRequestBehavior.AllowGet);

            db.Servicios.Remove(xservicio);

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
