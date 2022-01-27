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
using AutoMapper;
using MandaditosExpress.Models.ViewModels;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin, Asistente")]
    public class CostosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private CostoServices CostoServices;
        private IMapper mapper;

        public CostosController(IMapper mapper)
        {
            CostoServices = new CostoServices(db);
            this.mapper = mapper;
        }

        // GET: Costos
        public ActionResult Index()
        {
            var costos = mapper.Map<ICollection<CostoIndexViewModel>>(db.Costos.ToList());

            ViewBag.dt = JsonConvert.SerializeObject(costos.ToList());
            return View();
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
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            var tiposservicio = db.TiposDeServicio.Where(tp => !(tp.DescripcionTipoDeServicio.ToUpper().Contains("BANC")) && tp.EstadoTipoDeServicio).ToList();

            if (tiposservicio.Count <= 0)
                tiposservicio.Insert(0, new TipoDeServicio { Id=0,DescripcionTipoDeServicio = "-- Sin Registros --" });

            ViewBag.TipoDeServicioId = new SelectList(tiposservicio, "Id", "DescripcionTipoDeServicio");
            return View(new Costo());
        }

        // POST: Costos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Descripcion,FechaDeInicio,FechaDeFin,CostoDeGasolina,CostoDeAsistencia,CostoDeMotorizado,DistanciaBase,PrecioPorKm,TipoDeServicioId,EstadoDelCosto,PrecioDeRecargo,PrecioDeRegreso")] Costo costo)
        {
            if (ModelState.IsValid)
            {
                //activar el costo actual
                costo.EstadoDelCosto = true;

                // TODO validar antes de crear 
                var errorMessage = CostoServices.ValidarFechaCreate(costo.FechaDeInicio, costo.FechaDeFin, costo.TipoDeServicioId);

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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,FechaDeInicio,FechaDeFin,CostoDeGasolina,CostoDeAsistencia,CostoDeMotorizado,DistanciaBase,PrecioPorKm,TipoDeServicioId,EstadoDelCosto,PrecioDeRecargo,PrecioDeRegreso")] Costo costo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var error = CostoServices.validateFechaEdit(costo);

                    if (!string.IsNullOrEmpty(error))
                        return Json(new { exito = false, message = error }, JsonRequestBehavior.AllowGet);

                    var CostoInDb = db.Costos.Find(costo.Id);

                    if (CostoInDb != null)
                    {
                        //el controlador no esta recibiendo las propiedades de navegacion por lo que sino las lleva la detecta como si fuera una nueva entidad y no logra modificarla
                        //para resolver esto que me tiene hasta la madre hice esto aunque no es correcto
                        //asignar a la que ya esta en la bd los campos que se permiten editar en la vista
                        CostoInDb.Descripcion = costo.Descripcion;
                        CostoInDb.PrecioPorKm = costo.PrecioPorKm;
                        CostoInDb.DistanciaBase = costo.DistanciaBase;
                        CostoInDb.FechaDeFin = costo.FechaDeFin;
                        CostoInDb.CostoDeGasolina = costo.CostoDeGasolina;
                        CostoInDb.CostoDeAsistencia = costo.CostoDeAsistencia;
                        CostoInDb.CostoDeMotorizado = costo.CostoDeMotorizado;
                        CostoInDb.EstadoDelCosto = costo.EstadoDelCosto;
                        CostoInDb.PrecioDeRecargo = costo.PrecioDeRecargo;
                        CostoInDb.PrecioDeRegreso = costo.PrecioDeRegreso;

                        db.Entry(CostoInDb).State = EntityState.Modified;

                        if (db.SaveChanges() > 0)
                            return Json(new { exito = true }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { exito = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, message = "Ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Costos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Costo costo)
        {
            Costo xcosto = db.Costos.Find(costo.Id);

            db.Costos.Remove(xcosto);

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
