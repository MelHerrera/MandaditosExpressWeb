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
using MandaditosExpress.Models.Utileria;
using MandaditosExpress.Models.ViewModels;
using MandaditosExpress.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin, Asistente")]
    public class PersonasController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private ApplicationDbContext dbSecurity = new ApplicationDbContext();
        private ApplicationDbContext SecurityDB;
        private ApplicationUserManager UserManager;

        public PersonasController()
        {
            SecurityDB = new ApplicationDbContext();
        }

        // GET: Personas
        public ActionResult Index()
        {
            var Personas = GetUserList();

            ViewBag.Personas = JsonConvert.SerializeObject(Personas.ToList());
            return View();
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
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            //se excluye el rol admin por seguridad.
            //se excluye el rol motorizado porque habria que validar la vista para pedir ciertos datos que los tiene un motorizado pero no una persona.
            var roles = dbSecurity.Roles.Where(it=> it.Name.ToLower()!="Admin".ToLower() && it.Name.ToLower()!="Motorizado".ToLower()).ToList();

            ViewBag.Rol = new SelectList(roles, nameof(IRole.Name), nameof(IRole.Name));
            return View(new CreateUsuarioViewModel());
        }

        // POST: Personas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(CreateUsuarioViewModel Persona)
        {
            var user = new ApplicationUser();
            var PersonaToCreate = new Persona();
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            try
            {
                if (ModelState.IsValid)
                {
                    user = new ApplicationUser { UserName = Persona.CorreoElectronico, Email = Persona.CorreoElectronico, PhoneNumber = Persona.Telefono, EmailConfirmed = true };

                    var UserInDb = UserManager.FindByEmail(user.Email);

                    if (UserInDb == null)//aun no esta registrado
                    {
                        var result = await UserManager.CreateAsync(user, Persona.Password);

                        if (result.Succeeded)
                        {
                            await UserManager.AddToRoleAsync(user.Id, Persona.Rol);//el rol cliente debio ser creado en el startup.cs

                            //Agregamos la persona
                            PersonaToCreate = new Persona
                            {
                                CorreoElectronico = Persona.CorreoElectronico,
                                PrimerNombre = Persona.PrimerNombre,
                                SegundoNombre = Persona.SegundoNombre,
                                PrimerApellido = Persona.PrimerApellido,
                                SegundoApellido = Persona.SegundoApellido,
                                Telefono = Persona.Telefono,
                                Foto = new Utileria().getImageBytes(Request),
                                Sexo = Persona.Sexo,
                                Direccion = Persona.Direccion,
                                Cedula = Persona.Cedula,
                                FechaIngreso = DateTime.Now
                            };

                            db.Personas.Add(PersonaToCreate);

                            if (db.SaveChanges() > 0)
                                return RedirectToAction("Index");
                            else
                            {
                                UserManager.RemoveFromRole(user.Id, Persona.Rol);
                                UserManager.Delete(user);
                                ModelState.AddModelError("", "Sucedio un error procesando tu solicitud");
                            }
                        }
                        else
                            AddErrors(result);
                    }
                    else
                        ModelState.AddModelError("", "El correo electronico ingresado ya se encuentra registrado");
                }

                var roles = dbSecurity.Roles.Where(it => it.Name.ToLower() != "Admin".ToLower() && it.Name.ToLower() != "Motorizado".ToLower()).ToList();
                ViewBag.Rol = new SelectList(roles, nameof(IRole.Name), nameof(IRole.Name));
                return View(Persona);
            }
            catch (Exception ex)
            {
                //si sucede algun error interno entonces quitar la persona y el usuario.
                UserManager.RemoveFromRole(user.Id, Persona.Rol);
                UserManager.Delete(user);

                if (PersonaToCreate.Id > 0)
                {
                    db.Personas.Remove(PersonaToCreate);
                    db.SaveChanges();
                }

                throw new Exception("Ocurrio un error procesando tu solicitud!");
            }
        }

        // GET: Personas/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Persona persona = db.Personas.Find(id);
            db.Personas.Remove(persona);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CambiarContrasenia()
        {
            var Personas = GetUserList();

            ViewBag.Personas = JsonConvert.SerializeObject(Personas.ToList());
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DefaultPassword()
        {
            var Personas = GetUserList();

            ViewBag.Personas = JsonConvert.SerializeObject(Personas.ToList());
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> DefaultPassword(int PersonaId)
        {
            var Persona = db.Personas.Find(PersonaId);
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (Persona != null)
            {
                var User = SecurityDB.Users.FirstOrDefault(it => it.Email == Persona.CorreoElectronico);

                if (User != null)
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(User.Id);
                    var defaultPassword = Utilidades.GenerateDefaultPasswordByEmail(Persona.CorreoElectronico.Trim());
                    
                    var result = await UserManager.ResetPasswordAsync(User.Id, code, defaultPassword);

                    if (result.Succeeded)
                    {
                        return Json(new { exito = true, message = "Se establecio correctamente la contraseña por defecto al usuario. Contraseña: " + defaultPassword }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { exito = false, message = string.Join(" | ", result.Errors.ToList()) }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { exito = false, message = "Lo sentimos, ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmacionManual()
        {
            var Personas = GetUserList().Where(it=> it.EmailConfirmed==false);

            ViewBag.Personas = JsonConvert.SerializeObject(Personas.ToList());
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> ConfirmacionManual(int PersonaId)
        {
            var Persona = db.Personas.Find(PersonaId);
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (Persona != null)
            {
                var User = SecurityDB.Users.FirstOrDefault(it => it.Email == Persona.CorreoElectronico);

                if (User != null)
                {
                    if (!User.EmailConfirmed)
                    {
                        var code = await UserManager.GenerateEmailConfirmationTokenAsync(User.Id);

                        var result = await UserManager.ConfirmEmailAsync(User.Id, code);

                        if (result.Succeeded)
                            return Json(new { exito = true, message = "Se realizó la confirmación del correo exitosamente" }, JsonRequestBehavior.AllowGet);
                        else
                            return Json(new { exito = false, message = string.Join(" | ", result.Errors.ToList()).Length > 0 ? string.Join(" | ", result.Errors.ToList()) : "Ha ocurrido un error procesando su solicitud" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { exito = false, message = "El usuario seleccionado ya tiene confirmado el correo" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { exito = false, message = "Lo sentimos, ha sucedido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<UsuarioViewModel> GetUserList()
        {
            UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
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
                               EmailConfirmedDescripcion = usuarios.EmailConfirmed ? "Confirmado" : "Sin confirmar",
                               Rol = string.Join(" | ", UserManager.GetRoles(usuarios.Id))
                           };

            return Personas;
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
