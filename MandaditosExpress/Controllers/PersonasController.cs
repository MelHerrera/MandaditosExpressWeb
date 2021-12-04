using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MandaditosExpress.Models;
using MandaditosExpress.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonasController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private ApplicationDbContext SecurityDB;
        private ApplicationUserManager UserManager;

        public PersonasController()
        {
            SecurityDB = new ApplicationDbContext();
        }

        // GET: Personas
        public ActionResult Index()
        {
            return View(db.Personas.ToList());
        }

        // GET: Personas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Personas.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // GET: Personas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonaId,CorreoElectronico,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Telefono,Foto,Sexo,Direccion,Cedula,FechaIngresoDeLaPersona")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                db.Personas.Add(persona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(persona);
        }

        // GET: Personas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Personas.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonaId,CorreoElectronico,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Telefono,Foto,Sexo,Direccion,Cedula,FechaIngresoDeLaPersona")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(persona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(persona);
        }

        // GET: Personas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Personas.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Persona persona = db.Personas.Find(id);
            db.Personas.Remove(persona);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CambiarContrasenia()
        {
            var Usuarios = SecurityDB.Users.ToList();

            var Personas = from usuarios in Usuarios
                           join personas in db.Personas on usuarios.Email equals personas.CorreoElectronico
                           select new UsuarioViewModel
                           {
                               Id = personas.Id,
                               CorreoElectronico = personas.CorreoElectronico,
                               EmailConfirmed = usuarios.EmailConfirmed,
                               Telefono = personas.Telefono,
                               Foto = personas.Foto,
                               Nombres = personas.PrimerNombre + " " + personas.PrimerApellido + " " + personas.SegundoApellido,
                               EmailConfirmedClass = usuarios.EmailConfirmed ? "badge badge-primary" : "badge badge-warning",
                               EmailConfirmedDescripcion = usuarios.EmailConfirmed ? "Confirmado" : "Sin confirmar"
                           };

            ViewBag.Personas = JsonConvert.SerializeObject(Personas.ToList());
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CambiarContrasenia(int PersonaId, string NPassword)
        {
            var Persona = db.Personas.Find(PersonaId);
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (Persona != null)
            {
                var User = SecurityDB.Users.FirstOrDefault(it=> it.Email == Persona.CorreoElectronico);

                if (User !=null)
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(User.Id);

                    var result = await UserManager.ResetPasswordAsync(User.Id, code, NPassword);

                    if (result.Succeeded)
                    {
                        return Json(new { exito = true, message = "Se actualizó la contraseña del usuario correctamente" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { exito = false, message = string.Join(" | ", result.Errors.ToList()) }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { exito = false, message = "Lo sentimos, ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
         }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
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
