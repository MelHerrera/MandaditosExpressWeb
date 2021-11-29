using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using MandaditosExpress.Models.Utileria;
using System.Web.Mvc;
using MandaditosExpress.Models;
using MandaditosExpress.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AsistentesController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: Asistentes
        public ActionResult Index()
        {
            return View(db.Asistentes.ToList());
        }

        // GET: Asistentes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistente asistente = db.Asistentes.Find(id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // GET: Asistentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Asistentes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AsistenteViewModel asistente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (asistente.CorreoElectronico != null && asistente.Password != null)
                    {
                        var user = new ApplicationUser { UserName = asistente.CorreoElectronico, Email = asistente.CorreoElectronico, PhoneNumber = asistente.Telefono, EmailConfirmed = true };
                        var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                        var UserInDb = UserManager.FindByEmail(user.Email);

                        if (UserInDb == null)//aun no esta registrado
                        {
                            var result = await UserManager.CreateAsync(user, asistente.Password);

                            if (result.Succeeded)
                            {
                                //agregar a su correspondiente rol aqui
                                await UserManager.AddToRoleAsync(user.Id, "Asistente");//el rol Asistente debio ser creado en el startup.cs
                                //Agregamos el asistente
                                var asi = new Asistente
                                {
                                    CorreoElectronico = asistente.CorreoElectronico,
                                    EstadoDeAsistente = true,
                                    PrimerNombre = asistente.PrimerNombre,
                                    SegundoNombre = asistente.SegundoNombre,
                                    PrimerApellido = asistente.PrimerApellido,
                                    SegundoApellido = asistente.SegundoApellido,
                                    Telefono = asistente.Telefono,
                                    Foto = new Utileria().getImageBytes(Request),
                                    Sexo = asistente.Sexo,
                                    Direccion = asistente.Direccion,
                                    Cedula = asistente.Cedula,
                                    FechaIngreso = DateTime.Today,
                                    FechaDeBaja = asistente.FechaDeBaja
                                };

                                if (ModelState.IsValid)
                                {
                                    db.Asistentes.Add(asi);

                                    // agregar la validacion del Rol cuando se esten manejando roles en el sistema
                                    if (db.SaveChanges() > 0)
                                        return RedirectToAction("Login", "Account");
                                }

                            }
                            else
                                AddErrors(result);
                        }
                        else
                        {
                            var error = "El correo electronico ingresado ya se encuentra registrado";
                            ModelState.AddModelError("", error);
                        }


                    }
                }


                return View(asistente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ActionResult Create([Bind(Include = "Id,FechaIngresoDeAsistente,EstadoDeAsistente,FechaDeBaja,CorreoElectronico,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Telefono,Foto,Sexo,Direccion,Cedula,FechaIngreso")] Asistente asistente)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Asistentes.Add(asistente);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(asistente);
        //}

        // GET: Asistentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistente asistente = db.Asistentes.Find(id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // POST: Asistentes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FechaIngresoDeAsistente,EstadoDeAsistente,FechaDeBaja,CorreoElectronico,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Telefono,Foto,Sexo,Direccion,Cedula,FechaIngreso")] Asistente asistente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asistente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asistente);
        }

        // GET: Asistentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistente asistente = db.Asistentes.Find(id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // POST: Asistentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asistente asistente = db.Asistentes.Find(id);
            db.Asistentes.Remove(asistente);
            db.SaveChanges();
            return RedirectToAction("Index");
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