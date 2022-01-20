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
using AutoMapper;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin, Asistente")]
    public class ClientesController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private IMapper _mapper;

        public ClientesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: Clientes
        public ActionResult Index()
        {
            var data = GetUserList().ToList();
            ViewBag.Clientes = JsonConvert.SerializeObject(data);
            return View();
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
            return View(new ClienteViewModel());
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClienteViewModel cliente)
        {
            var user = new ApplicationUser();
            var cl = new Cliente();
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            try
            {
                if (ModelState.IsValid)
                {
                    //si esta chekeado como empresa entonces validar manualmente el RUC y Nombre de empresa
                    if (cliente.EsEmpresa)
                    {
                        if(cliente.RUC==null || cliente.NombreDeLaEmpresa==null || cliente.NombreDeLaEmpresa.Length <= 0)
                        {
                            ModelState.AddModelError("", "El número RUC y nombre del negocio es obligatorio");
                            return View(cliente);
                        }
                        if (cliente.RUC.Length != 14)
                        {
                            ModelState.AddModelError("", "El número RUC debe tener 14 caracteres de longitud");
                            return View(cliente);
                        }
                    }

                    if (cliente.CorreoElectronico != null && cliente.Password != null)
                    {
                        user = new ApplicationUser { UserName = cliente.CorreoElectronico, Email = cliente.CorreoElectronico, PhoneNumber = cliente.Telefono };

                        var UserInDb = UserManager.FindByEmail(user.Email);

                        if (UserInDb == null)//aun no esta registrado
                        {
                            var result = await UserManager.CreateAsync(user, cliente.Password);

                            if (result.Succeeded)
                            {
                                //agregar a su correspondiente rol aqui
                                await UserManager.AddToRoleAsync(user.Id, "Cliente");//el rol cliente debio ser creado en el startup.cs

                                //Agregamos el cliente
                                cl = new Cliente
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
                                    FechaIngreso = DateTime.Now,
                                    EsEmpresa = cliente.EsEmpresa,
                                    NombreDeLaEmpresa = cliente.NombreDeLaEmpresa,
                                    RUC = cliente.RUC
                                };

                                if (ModelState.IsValid)
                                {
                                    db.Clientes.Add(cl);

                                    if (db.SaveChanges() > 0)
                                    {
                                        //si se ha guardado bien el usuario, rol y cliente entonces enviar el correo
                                        //Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771
                                        //Enviar correo electrónico con este vínculo
                                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                                        string confirmationMessageBody = string.Format("Estimado cliente para confirmar tu cuenta, haz clic  {0}", "<a href='" + callbackUrl + "'>Aquí</a>");
                                        await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta", confirmationMessageBody);

                                        return RedirectToAction("Login", "Account");
                                    }
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

                return View(cliente);
            }
            catch (Exception ex)
            {
                //si sucede algun error interno entonces quitar el cliente y el usuario
                UserManager.RemoveFromRole(user.Id, "Cliente");
                UserManager.Delete(user);

                if(cl.Id > 0)
                {
                    db.Clientes.Remove(cl);
                    db.SaveChanges();
                }

                throw new Exception("Ocurrio un error procesando tu solicitud!");
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,EsEmpresa,NombreDeLaEmpresa,RUC,FechaIngresoDelCliente,CorreoElectronico,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Telefono,Foto,Sexo,Direccion,Cedula,FechaIngreso")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    var data = GetUserList().ToList();
                    ViewBag.Clientes = JsonConvert.SerializeObject(data);
                    return View("Index");
                }
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
        [Authorize(Roles ="Admin")]
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

        public IEnumerable<IndexClienteViewModel> GetUserList()
        {
            using (var SecurityDb = new ApplicationDbContext())
            {
                var Usuarios = SecurityDb.Users.ToList();

                var ListaClientes = from usuarios in Usuarios
                               join clientes in db.Clientes on usuarios.Email equals clientes.CorreoElectronico
                               select new IndexClienteViewModel
                               {
                                   Id = clientes.Id,
                                   CorreoElectronico = clientes.CorreoElectronico,
                                   Telefono = clientes.Telefono,
                                   Foto = clientes.Foto,
                                   Nombres = clientes.PrimerNombre + " " + clientes.PrimerApellido + " " + clientes.SegundoApellido,
                                   Direccion= clientes.Direccion,
                                   TipoDePersona = clientes.EsEmpresa ? "Negocio" : "Persona",
                                   TipoDePersonaClass = clientes.EsEmpresa ? "badge badge-warning" : "badge badge-success",
                                   EmailConfirmed = usuarios.EmailConfirmed,
                                   EmailConfirmedClass = usuarios.EmailConfirmed ? "badge badge-primary" : "badge badge-warning",
                                   EmailConfirmedDescripcion = usuarios.EmailConfirmed ? "Confirmado" : "Sin confirmar"
                               };

                return ListaClientes;
            }
        }
    }
}
