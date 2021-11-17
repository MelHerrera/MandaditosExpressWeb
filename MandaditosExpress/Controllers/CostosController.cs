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
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio.Where(tp => !(tp.DescripcionTipoDeServicio.ToUpper().Contains("BANC"))), "Id", "DescripcionTipoDeServicio");

            return View(costo);
        }

        // GET: Costos/Create
        public ActionResult Create()
        {
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio.Where(tp => !(tp.DescripcionTipoDeServicio.ToUpper().Contains("BANC"))), "Id", "DescripcionTipoDeServicio");
            return View(new Costo());
        }

        // POST: Costos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,FechaDeInicio,FechaDeFin,CostoDeGasolina,CostoDeAsistencia,CostoDeMotorizado,DistanciaBase,PrecioPorKm,TipoDeServicioId,EstadoDelCosto,PrecioDeRecargo,PrecioDeRegreso")] Costo costo)
        {
            if (ModelState.IsValid)
            {
                //activar el costo actual
                costo.EstadoDelCosto = true;

                // TODO validar antes de crear 
                var errorMessage = ValidarCreate(costo.FechaDeInicio, costo.FechaDeFin, costo.TipoDeServicioId);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    //desactivar el costo de ese mismo tipo que ya estaban, para que no hayan dos costos para el mismo tipo de servicio.
                    var CostosAntiguo = (from c in db.Costos
                                         where c.TipoDeServicioId == costo.TipoDeServicioId && c.EstadoDelCosto
                                         select c).ToList();

                    CostosAntiguo.ForEach(x => x.EstadoDelCosto = false);

                    db.Costos.Add(costo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError("", errorMessage);
            }

            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio.Where(tp => !(tp.DescripcionTipoDeServicio.ToUpper().Contains("BANC"))), "Id", "DescripcionTipoDeServicio");
            return View(costo);
        }

        // POST: Costos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,FechaDeInicio,FechaDeFin,CostoDeGasolina,CostoDeAsistencia,CostoDeMotorizado,DistanciaBase,PrecioPorKm,TipoDeServicioId,EstadoDelCosto,PrecioDeRecargo,PrecioDeRegreso")] Costo costo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costo).State = EntityState.Modified;

                if (db.SaveChanges() > 0)
                    return Json(new { exito = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { exito = false }, JsonRequestBehavior.AllowGet);
        }

        // POST: Costos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Costo costo)
        {
            Costo xcosto = db.Costos.Find(costo.Id);
            db.Costos.Remove(xcosto);

            if (db.SaveChanges() > 0)
                return Json(new { exito = true }, JsonRequestBehavior.AllowGet);

            return Json(new { exito = false }, JsonRequestBehavior.AllowGet);
        }

        public string ValidarCreate(DateTime FIncicio, DateTime Ffin, int TipoDeServicioId)
        {
            // fecha de inicio no puede ser menor por mas de 3 minutos a la fecha actual
            // fecha inicio no puede ser mayor o igual a la fecha de fin
            // fecha fin no puede ser menor o igual a la fecha de inicio
            // fecha inicio no puede ser menos a una de las fechas de inicio de los costos vigentes para el mismo tipo de servicio
            // la fecha fin no puede ser menor a una de las fechas de inicio de los costos vigentes para el mismo tipo de servicio
            // la fecha fin no puede ser menor o igual a una de las fechas de fin de los costos vigentes para el mismo tipo de servicio

            var error = string.Empty;

            if (FIncicio.AddMinutes(3) < DateTime.Now)//permitirle que la fecha de inicio solo pueda ser 3 minutos antes de la hora y fecha actual
                return error = "La fecha de inicio debe ser igual o mayor  a la fecha y hora actual. No puede iniciar un costo en un instante de tiempo pasado";
            if (FIncicio >= Ffin)
                return error = "La fecha de inicio no puede ser mayor o igual a la fecha de finalización del costo";
            if (Ffin <= FIncicio)
                return error = "La fecha de fin no puede ser menor o igual a la fecha de inicio del costo";

            var CostosAntiguo = (from c in db.Costos
                                 where c.TipoDeServicioId == TipoDeServicioId && c.EstadoDelCosto
                                 select c).ToList();

            if (CostosAntiguo.Any(it => it.TipoDeServicioId == TipoDeServicioId && it.EstadoDelCosto && FIncicio <= it.FechaDeInicio))
                return error = "No se puede agregar un costo cuya fecha de inicio sea menor a la fecha de inicio del costo(s) vigente";

            if (CostosAntiguo.Any(it => it.TipoDeServicioId == TipoDeServicioId && it.EstadoDelCosto && Ffin <= it.FechaDeInicio))
                return error = "No se puede agregar un costo cuya fecha de finalizacion sea menor a la fecha de inicio del costo(s) vigente";

            if (CostosAntiguo.Any(it => it.TipoDeServicioId == TipoDeServicioId && it.EstadoDelCosto && Ffin <= it.FechaDeFin))
                return error = "No se puede agregar un costo cuya fecha de finalizacion sea menor a la fecha de fim del costo(s) vigente";

            return error;
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
