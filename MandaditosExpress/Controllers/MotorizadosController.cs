using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;

namespace MandaditosExpress.Controllers
{
    [Authorize]
    public class MotorizadosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: Motorizados
        public ActionResult Index()
        {
            return View(db.Motorizados.ToList());
        }

        // GET: Motorizados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motorizado motorizado = db.Motorizados.Find(id);
            if (motorizado == null)
            {
                return HttpNotFound();
            }
            return View(motorizado);
        }

        // GET: Motorizados/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            var Estados = Enum.GetValues(typeof(EstadoDeAfiliadoEnum));

            if (Request.IsAuthenticated && User.IsInRole("Admin"))
                ViewBag.EstadoDeAfiliado = Estados;
            else
                ViewBag.EstadoDeAfiliado = new List<EstadoDeAfiliadoEnum>() {EstadoDeAfiliadoEnum.Solicitud };

            return View();
        }

        // POST: Motorizados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EsAfiliado,EstadoDeAfiliado,FechaDeAfiliacion,FechaIngresoDelMotorizado,EstadoDeMotorizado,CorreoElectronico,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Telefono,Foto,Sexo,Direccion,Cedula,FechaIngreso")] Motorizado motorizado)
        {
            if (ModelState.IsValid)
            {
                db.Motorizados.Add(motorizado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(motorizado);
        }

        // GET: Motorizados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motorizado motorizado = db.Motorizados.Find(id);
            if (motorizado == null)
            {
                return HttpNotFound();
            }
            return View(motorizado);
        }

        // POST: Motorizados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EsAfiliado,EstadoDeAfiliado,FechaDeAfiliacion,FechaIngresoDelMotorizado,EstadoDeMotorizado,CorreoElectronico,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Telefono,Foto,Sexo,Direccion,Cedula,FechaIngreso")] Motorizado motorizado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(motorizado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(motorizado);
        }

        // GET: Motorizados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motorizado motorizado = db.Motorizados.Find(id);
            if (motorizado == null)
            {
                return HttpNotFound();
            }
            return View(motorizado);
        }

        // POST: Motorizados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Motorizado motorizado = db.Motorizados.Find(id);
            db.Motorizados.Remove(motorizado);
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
