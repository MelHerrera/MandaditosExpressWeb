using System;
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
    [Authorize(Roles = "Admin")]
    public class TipoDeServiciosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private IMapper _mapper;

        public TipoDeServiciosController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: TipoDeServicios
        public ActionResult Index()
        {
            return View(_mapper.Map<ICollection<TipoDeServicioViewModel>>(db.TiposDeServicio.ToList()));
        }

        // GET: TipoDeServicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeServicio tipoDeServicio = db.TiposDeServicio.Find(id);
            if (tipoDeServicio == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeServicio);
        }

        // GET: TipoDeServicios/ ate
        public ActionResult Create()
        {
            return View(new TipoDeServicio());
        }

        // POST: TipoDeServicios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DescripcionTipoDeServicio,EstadoTipoDeServicio")] TipoDeServicio tipoDeServicio)
        {
            if (ModelState.IsValid)
            {
                db.TiposDeServicio.Add(tipoDeServicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDeServicio);
        }


        // POST: TipoDeServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DescripcionTipoDeServicio,EstadoTipoDeServicio")] TipoDeServicio tipoDeServicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDeServicio).State = EntityState.Modified;

                if (db.SaveChanges() > 0)
                    return Json(new { exito = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { exito = false }, JsonRequestBehavior.AllowGet);
        }

        // POST: TipoDeServicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(TipoDeServicio tipoDeServicio)
        {
          TipoDeServicio xtiposdeservicio = db.TiposDeServicio.Find(tipoDeServicio.Id);

            if(xtiposdeservicio.Costos.Count > 0 || xtiposdeservicio.CostosGestionBancaria.Count > 0 || xtiposdeservicio.Cotizaciones.Count > 0 || xtiposdeservicio.Servicios.Count > 0)
                return Json(new { exito = false, message = "No se puede eliminar el tipo de servicio porque tiene registros asociados" }, JsonRequestBehavior.AllowGet);

            db.TiposDeServicio.Remove(xtiposdeservicio);

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
