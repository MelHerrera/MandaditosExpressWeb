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
    [Authorize(Roles = "Admin, Cliente")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Descripcion,FechaDeInicio,FechaDeVencimiento,EstadoDelCredito,FechaDeCancelacion,ClienteId")] Credito credito)
        {
            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", credito.ClienteId);

            if (ModelState.IsValid)
            {
                //validaciones sobre el credito a guardar
                ////creditos del cliente en el mismo periodo y que sean creditos que no tengan pagos
                //var ExisteCreditoMismoPeriodo = db.Creditos.FirstOrDefault(it=> it.FechaDeInicio>=credito.FechaDeInicio && it.FechaDeVencimiento<=credito.FechaDeInicio &&
                //                                                           it.FechaDeInicio<=credito.FechaDeVencimiento && 
                //                                                           it.ClienteId == credito.ClienteId && it.Pagos.Count<= 0);
                //if (ExisteCreditoMismoPeriodo != null)
                //{
                //    ModelState.AddModelError("","No se puede grabar otro credito a este cliente en el mismo periodo");
                //    return View(credito);
                //}

                db.Creditos.Add(credito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(credito);
        }

        // POST: Creditos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,FechaDeInicio,FechaDeVencimiento,EstadoDelCredito,FechaDeCancelacion,ClienteId")] Credito credito)
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
        [Authorize(Roles = "Admin")]
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
