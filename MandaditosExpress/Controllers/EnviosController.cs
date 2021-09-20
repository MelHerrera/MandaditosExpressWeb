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
    public class EnviosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

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
            ViewBag.AsistenteId = new SelectList(db.Personas, "Id", "CorreoElectronico");
            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico");
            ViewBag.MotocicletaId = new SelectList(db.Motocicletas, "Id", "Placa");
            ViewBag.MotorizadoId = new SelectList(db.Personas, "Id", "CorreoElectronico");
            ViewBag.ServicioId = new SelectList(db.Servicios, "Id", "DescripcionDelServicio");
            return View();
        }

        // POST: Envios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DescripcionDeEnvio,FechaDelEnvio,DireccionDeRecepcion,DireccionDeEntrega,DistanciaEntregaRecep,NombresDelReceptor,ApellidosDelReceptor,CedulaDelReceptor,Ancho,Alto,Peso,MontoDeDinero,TelefonoDelReceptor,FechaDeEntrega,EsUrgente,HoraDeEntrega,PrecioDeRecargo,EstadoDelEnvio,MotocicletaId,AsistenteId,ClienteId,MotorizadoId,Credito,ServicioId")] Envio envio)
        {
            if (ModelState.IsValid)
            {
                db.Envios.Add(envio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsistenteId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.AsistenteId);
            ViewBag.ClienteId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.ClienteId);
            ViewBag.MotocicletaId = new SelectList(db.Motocicletas, "Id", "Placa", envio.MotocicletaId);
            ViewBag.MotorizadoId = new SelectList(db.Personas, "Id", "CorreoElectronico", envio.MotorizadoId);
            ViewBag.ServicioId = new SelectList(db.Servicios, "Id", "DescripcionDelServicio", envio.ServicioId);
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
            ViewBag.ServicioId = new SelectList(db.Servicios, "Id", "DescripcionDelServicio", envio.ServicioId);
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
            ViewBag.ServicioId = new SelectList(db.Servicios, "Id", "DescripcionDelServicio", envio.ServicioId);
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
