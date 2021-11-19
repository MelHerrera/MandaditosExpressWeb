using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MandaditosExpress.Models;
using MandaditosExpress.Services;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CostosGestionBancariaController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private CostoBancarioServices CostoBancarioServices;

        public CostosGestionBancariaController()
        {
            CostoBancarioServices = new CostoBancarioServices(db);
        }
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
        public ActionResult Create([Bind(Include = "Id,FechaDeInicio,FechaDeFin,Descripcion,MontoDesde,MontoHasta,Estado,Porcentaje,valor,PrecioDeRecargo,PrecioDeRegreso,TipoDeServicioId")] CostoGestionBancaria costoGestionBancaria)
        {
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio.Where(tp => tp.DescripcionTipoDeServicio.ToUpper().Contains("BANC")), "Id", "DescripcionTipoDeServicio");


            if (ModelState.IsValid)
            {
                if (costoGestionBancaria.Porcentaje <= 0 && costoGestionBancaria.Valor <= 0)
                    ModelState.AddModelError("", "Debe especificar el porcentaje a o el valor a cobrar por la gestion bancaria");

                if (costoGestionBancaria.Porcentaje > 0 && costoGestionBancaria.Valor > 0)
                    ModelState.AddModelError("", "No puede especificar el porcentaje y valor a la vez, solo se debe especificar uno de ellos.");

                //activar el costo actual
                costoGestionBancaria.Estado = true;
                costoGestionBancaria.Valor = costoGestionBancaria.Valor > 0 ? costoGestionBancaria.Valor : 0;
                costoGestionBancaria.Porcentaje = costoGestionBancaria.Porcentaje > 0 ? costoGestionBancaria.Porcentaje : 0;

                // TODO validar antes de crear 
                var errorMessage = CostoBancarioServices.ValidarFechaCreate(costoGestionBancaria.FechaDeInicio, costoGestionBancaria.FechaDeFin, costoGestionBancaria.TipoDeServicioId);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    //desactivar el costo de ese mismo tipo que ya estaban, para que no hayan dos costos para el mismo tipo de servicio.
                    var CostosAntiguo = (from c in db.CostoGestionBancaria
                                         where c.TipoDeServicioId == costoGestionBancaria.TipoDeServicioId && c.Estado
                                         select c).ToList();

                    CostosAntiguo.ForEach(x => x.Estado = false);

                    db.CostoGestionBancaria.Add(costoGestionBancaria);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError("", errorMessage);
            }

            return View(costoGestionBancaria);
        }

        // POST: CostosGestionBancaria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CostoGestionBancaria costoGestionBancaria)
        {
            ViewBag.TipoDeServicioId = new SelectList(db.TiposDeServicio.Where(tp => tp.DescripcionTipoDeServicio.ToUpper().Contains("BANC")), "Id", "DescripcionTipoDeServicio");

            if (ModelState.IsValid)
            {
                var error = CostoBancarioServices.validateFechaEdit(costoGestionBancaria);

                if (!string.IsNullOrEmpty(error))
                    return Json(new { exito = false, message = error }, JsonRequestBehavior.AllowGet);

                var CostoInDb = db.CostoGestionBancaria.Find(costoGestionBancaria.Id);

                if (CostoInDb != null)
                {
                    CostoInDb.Descripcion = costoGestionBancaria.Descripcion;
                    CostoInDb.FechaDeFin = costoGestionBancaria.FechaDeFin;
                    CostoInDb.MontoDesde = costoGestionBancaria.MontoDesde;
                    CostoInDb.MontoHasta = costoGestionBancaria.MontoHasta;
                    CostoInDb.Porcentaje = costoGestionBancaria.Porcentaje;
                    CostoInDb.Valor = costoGestionBancaria.Valor;
                    CostoInDb.PrecioDeRecargo = costoGestionBancaria.PrecioDeRecargo;
                    CostoInDb.PrecioDeRegreso = costoGestionBancaria.PrecioDeRegreso;
                    CostoInDb.Estado = costoGestionBancaria.Estado;

                    db.Entry(CostoInDb).State = EntityState.Modified;

                    if (db.SaveChanges() > 0)
                        return Json(new { exito = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { exito = false, message = "Ocurrió un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
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
