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
    [Authorize]
    public class ClientesController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: Clientes
        public ActionResult Index()
        {
            return View(db.Clientes.ToList());

        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        [AllowAnonymous]
        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClienteViewModel cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (cliente.CorreoElectronico != null && cliente.Password != null)
                    {
                        var user = new ApplicationUser { UserName = cliente.CorreoElectronico, Email = cliente.CorreoElectronico, PhoneNumber = cliente.Telefono};
                        var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                        var UserInDb = UserManager.FindByEmail(user.Email);

                        if (UserInDb == null)//aun no esta registrado
                        {
                            var result = await UserManager.CreateAsync(user, cliente.Password);

                            if (result.Succeeded)
                            {
                                //Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771
                                //Enviar correo electrónico con este vínculo
                                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                                string confirmationMessageBody = string.Format("Estimado cliente para confirmar tu cuenta, haz clic  {0}", "<a href='" + callbackUrl + "'>Aquí</a>");
                                await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta", confirmationMessageBody);

                                //agregar a su correspondiente rol aqui
                                await UserManager.AddToRoleAsync(user.Id, "Cliente");//el rol cliente debio ser creado en el startup.cs
                                //Agregamos el cliente
                                var cl = new Cliente
                                {
                                    CorreoElectronico = cliente.CorreoElectronico,
                                    PrimerNombre = cliente.PrimerNombre,
                                    SegundoNombre = cliente.SegundoNombre,
                                    PrimerApellido = cliente.PrimerApellido,
                                    SegundoApellido = cliente.SegundoApellido,
                                    Telefono = cliente.Telefono,
                                    Foto = new Utileria().getImageBytes(Request),
                                    Sexo = cliente.Sexo,
                                    Direccion = cliente.Direccion,
                                    Cedula = cliente.Cedula,
                                    FechaIngreso = DateTime.Today,
                                    EsEmpresa = cliente.EsEmpresa,
                                    NombreDeLaEmpresa = cliente.NombreDeLaEmpresa,
                                    RUC = cliente.RUC
                                };

                                if (ModelState.IsValid)
                                {
                                    db.Clientes.Add(cl);

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
                {
                    if (cliente.RUC != null && cliente.RUC.Length > 14)
                        ModelState.AddModelError("", "El número RUC no debe exeder los 14 caracteres de longitud");
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EsEmpresa,NombreDeLaEmpresa,RUC,FechaIngresoDelCliente,CorreoElectronico,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Telefono,Foto,Sexo,Direccion,Cedula,FechaIngreso")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return View("Index", db.Clientes.ToList());
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
