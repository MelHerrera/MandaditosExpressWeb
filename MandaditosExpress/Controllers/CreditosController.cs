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
using MandaditosExpress.Models.Utileria;
using MandaditosExpress.Models.ViewModels;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin, Cliente")]
    public class CreditosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private IMapper _mapper;
        private ApplicationDbContext SecurityDb = new ApplicationDbContext();

        public CreditosController(IMapper mapper)
        {
            _mapper = mapper;
        }
        // GET: Creditos
        public ActionResult Index()
        {
            var creditos = new List<Credito>();

            if(User.IsInRole("Admin"))
            creditos = db.Creditos.Include(c => c.Cliente).ToList();
            else
            {
                var UserName = Request.GetOwinContext().Authentication.User.Identity.Name;
                var PersonaActual = new Utileria().BuscarPersonaPorUsuario(UserName);

                //creditos filtrados por usuario
                creditos = db.Creditos.Include(c => c.Cliente).Where(it=> it.ClienteId == PersonaActual.Id).ToList();
            }

            ViewBag.dt = JsonConvert.SerializeObject(_mapper.Map<ICollection<CreditoViewModel>>(creditos));

            return View(new CreditoViewModel());
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
            var users = SecurityDb.Users.Where(it=> it.EmailConfirmed).Select(it=> it.Email).ToList();//todos los usuarios que ya esten confirmados
            var clientes = db.Clientes.Where(it=> users.Contains(it.CorreoElectronico)).ToList();
            ViewBag.ClienteId = new SelectList(clientes, "Id", "NombreCompleto");

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
            var users = SecurityDb.Users.Where(it => it.EmailConfirmed).Select(it => it.Email).ToList();//todos los usuarios que ya esten confirmados
            var clientes = db.Clientes.Where(it => users.Contains(it.CorreoElectronico)).ToList();
            ViewBag.ClienteId = new SelectList(clientes, "Id", "NombreCompleto");

            if (ModelState.IsValid)
            {
                // validaciones sobre las fechas del credito a guardar
                //puede ser 1 minuto menor a la fecha actual.
                //escenario: elije en el calendario hoy pero en todo lo que llena los otros campos la datetime.now sera mayor y entonces
                //la validacion se disparara, para evitar eso se le agrego 2 minuto de tiempo a la fecha de incio
                if (credito.FechaDeInicio.AddMinutes(2) < DateTime.Now)
                {
                    ModelState.AddModelError("", "La fecha de inicio debe ser mayor o igual a la fecha actual");
                    return View(credito);
                }
                if (credito.FechaDeInicio >= credito.FechaDeVencimiento)
                {
                    ModelState.AddModelError("", "La fecha de inicio no puede ser mayor o igual a la fecha de vencimiento");
                    return View(credito);
                }

                credito.EstadoDelCredito = true;
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

            if(Ocredito.Envios.Count > 0)
                return Json(new { exito = false, message = "No se puede eliminar el crédito porque tiene envios asociados" }, JsonRequestBehavior.AllowGet);

            if (Ocredito.Pagos.Count > 0)
                return Json(new { exito = false, message = "No se puede eliminar el crédito porque tiene pagos asociados" }, JsonRequestBehavior.AllowGet);

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
