using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Utileria;
using MandaditosExpress.Models.ViewModels;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize]
    public class EnviosController : Controller
    {
        private MandaditosDB db;
        private Utileria Utileria;

        public EnviosController()
        {
            db = new MandaditosDB();
            Utileria = new Utileria();
        }

        // GET: Envios
        public ActionResult Index()
        {
            var envios = db.Envios.Include(e => e.Asistente).Include(e => e.Cliente).Include(e => e.Motocicleta).Include(e => e.Motorizado).Include(e => e.Servicio);
            return View(envios.ToList());
        }

        // GET: Envios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envios.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            return View(envio);
        }

        // GET: Envios/Create
        public ActionResult Create()
        {
            var EnvioViewModel = new EnvioViewModel();

            //sacar el Id del Cliente que esta haciendo la solicitud del envio
            var CurrentUser = Request.GetOwinContext().Authentication.User.Identity.Name;
            var Cliente = Utileria.GetClienteByUser(CurrentUser);

            EnvioViewModel.ClienteId = Cliente!= null ? Cliente.Id : -1;

            return View(EnvioViewModel);
        }

        // POST: Envios/Createss
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EnvioViewModel envio)
        {
            if (ModelState.IsValid)
            {
                //db.Envios.Add(envio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(envio);
        }

        // GET: Envios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envios.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsistenteId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.AsistenteId);
            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.ClienteId);
            ViewBag.MotocicletaId = new SelectList(db.Motocicletas, "Id", "Placa", envio.MotocicletaId);
            ViewBag.MotorizadoId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.MotorizadoId);
            //ViewBag.ServicioId = new SelectList(db.Servicios, "Id", "DescripcionDelServicio", envio.ServicioId);
            return View(envio);
        }

        // POST: Envios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DescripcionDeEnvio,FechaDelEnvio,DireccionDeRecepcion,DireccionDeEntrega,DistanciaEntregaRecep,NombresDelReceptor,ApellidosDelReceptor,CedulaDelReceptor,Ancho,Alto,Peso,MontoDeDinero,TelefonoDelReceptor,FechaDeEntrega,EsUrgente,HoraDeEntrega,PrecioDeRecargo,EstadoDelEnvio,MotocicletaId,AsistenteId,ClienteId,MotorizadoId,Credito,ServicioId")] Envio envio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(envio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsistenteId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.AsistenteId);
            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.ClienteId);
            ViewBag.MotocicletaId = new SelectList(db.Motocicletas, "Id", "Placa", envio.MotocicletaId);
            ViewBag.MotorizadoId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.MotorizadoId);
            //ViewBag.ServicioId = new SelectList(db.Servicios, "Id", "DescripcionDelServicio", envio.ServicioId);
            return View(envio);
        }

        // GET: Envios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Envio envio = db.Envios.Find(id);
            if (envio == null)
            {
                return HttpNotFound();
            }
            return View(envio);
        }

        // POST: Envios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Envio envio = db.Envios.Find(id);
            db.Envios.Remove(envio);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult FiltrarServicio(int id)
        {

            if (id > 0)
            {
                List<Servicio> servicios = db.Servicios.Where(it => it.TipoDeServicioId == id).ToList()
                    .Select(y=>new Servicio() {
                    Id=y.Id,
                    DescripcionDelServicio=y.DescripcionDelServicio,
                    Estado=y.Estado,
                    TipoDeServicioId=y.TipoDeServicioId
                    }).ToList();

                return Json(new { exito = true, data = servicios }, JsonRequestBehavior.AllowGet);

            }

            return Json(false,JsonRequestBehavior.AllowGet);
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
