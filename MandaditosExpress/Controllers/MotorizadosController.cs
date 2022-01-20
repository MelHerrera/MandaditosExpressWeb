using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;
using MandaditosExpress.Models.Utileria;
using MandaditosExpress.Models.ViewModels;
using MandaditosExpress.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin, Asistente")]
    public class MotorizadosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private IMapper _mapper;

        public MotorizadosController(IMapper mapper)
        {
            _mapper = mapper; 
        }

        // GET: Motorizados
        public ActionResult Index()
        {
            return View(db.Motorizados.ToList());
        }

        // GET: Motorizados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Motorizado motorizado = db.Motorizados.Find(id);
            if (motorizado == null)
            {
                return HttpNotFound();
            }
            return View(motorizado);
        }

        // GET: Motorizados/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            var MotorizadoVM = new MotorizadoViewModel();
            var Disponibilidades = db.Disponibilidad.Where(x=>x.EstadoDeLaDisponibilidad).ToList();
            var Calidades = db.VelocidadDeConexion.Where(x=>x.Estado).ToList();

            if (Disponibilidades.Count <= 0)
                Disponibilidades.Insert(0, new Disponibilidad() { Id = -1, Descripcion = "--Sin Registros--" });

            if (Calidades.Count <= 0)
                Calidades.Insert(0, new CalidadDeConexion() { Id = -1, Descripcion = "--Sin Registros--" });

            ViewBag.DisponibilidadId = new SelectList(Disponibilidades, "Id", "Descripcion");
            ViewBag.VelocidadDeConexionId = new SelectList(Calidades, "Id", "Descripcion");

            return View(MotorizadoVM);
        }

        // POST: Motorizados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MotorizadoViewModel motorizado)
        {
            var Disponibilidades = db.Disponibilidad.Where(x=>x.EstadoDeLaDisponibilidad).ToList();
            var Calidades = db.VelocidadDeConexion.Where(x=>x.Estado).ToList();

            if (Disponibilidades.Count <= 0)
                Disponibilidades.Insert(0, new Disponibilidad() { Id = -1, Descripcion = "--Sin Registros--" });

            if (Calidades.Count <= 0)
                Calidades.Insert(0, new CalidadDeConexion() { Id = -1, Descripcion = "--Sin Registros--" });

            ViewBag.DisponibilidadId = new SelectList(Disponibilidades, "Id", "Descripcion", motorizado.DisponibilidadId);
            ViewBag.VelocidadDeConexionId = new SelectList(Calidades, "Id", "Descripcion", motorizado.VelocidadDeConexionId);


            if (ModelState.IsValid)
            {
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var UserInDb = UserManager.FindByEmail(motorizado.CorreoElectronico);

                //si es admin la contraseña debe ser obligatoria
                if (Request.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("Asistente")))
                {
                    if (motorizado.Password == null || motorizado.ConfirmPassword == null)
                    {
                        ModelState.AddModelError("", "El campo contraseña y confirmar contraseña son obligatorios");
                        return View(motorizado);
                    }

                    if (UserInDb != null)//si en la bd de seguridad ya existe un motorizado con ese correo
                    {
                        ModelState.AddModelError("", "El correo electronico proporcionado ya se encuentra en uso");
                        return View(motorizado);
                    }
                }

                //si no es un admin entonces la disponibilidad y velocidad de conexion son obligatorias
                if (!User.IsInRole("Admin") && !User.IsInRole("Asistente"))
                {
                    if (motorizado.DisponibilidadId <= 0 && motorizado.VelocidadDeConexionId <= 0)
                    {
                        ModelState.AddModelError("", "La Disponibilidad y Calidad de conexión a internet es obligatoria");
                        return View(motorizado);
                    }
                }
                if (motorizado.Cedula != null)
                {
                    if (motorizado.Cedula.Length != 16)
                    {
                        ModelState.AddModelError("", "El número de cédula proporcionado debe tener una longitud de 16 caracteres");
                        return View(motorizado);
                    }
                }

                var Motorizado = new Motorizado
                {
                    CorreoElectronico = motorizado.CorreoElectronico,
                    PrimerNombre = motorizado.PrimerNombre,
                    SegundoNombre = motorizado.SegundoNombre,
                    PrimerApellido = motorizado.PrimerApellido,
                    SegundoApellido = motorizado.SegundoApellido,
                    Telefono = motorizado.Telefono,
                    Foto = new Utileria().getImageBytes(Request),
                    Sexo = motorizado.Sexo,
                    Direccion = motorizado.Direccion,
                    Cedula = motorizado.Cedula,
                    FechaIngreso = DateTime.Now,
                    EsAfiliado = (Request.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("Asistente"))) ? false : true,
                    EstadoDeAfiliado = (Request.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("Asistente"))) ? (short)EstadoDeAfiliadoEnum.NoAplica : ((short)EstadoDeAfiliadoEnum.Solicitud),
                    VelocidadDeConexionId = motorizado.VelocidadDeConexionId > 0 ? motorizado.VelocidadDeConexionId : new int?(),
                    DisponibilidadId = motorizado.DisponibilidadId > 0 ? motorizado.DisponibilidadId : new int?(),
                    FechaDeAfiliacion = DateTime.Parse("01/01/1900 00:00:00")
                };

                Motorizado = db.Motorizados.Add(Motorizado);

                if (db.SaveChanges() > 0)
                {
                    var Motocicleta = new Motocicleta
                    {
                        Placa = motorizado.Placa,
                        Color = motorizado.Color,
                        Modelo = motorizado.Modelo,
                        Anio = motorizado.Anio,
                        EsPropia = motorizado.EsPropia,
                        Kilometraje = motorizado.Kilometraje,
                        FechaDeIngreso = DateTime.Now,
                        EsTemporal = false,
                        MotorizadoId = Motorizado.Id,
                        EstadoDeMotocicleta = true,
                        FechaDeValidez = DateTime.Parse("01/01/1900 00:00:00")
                    };

                    Motocicleta = db.Motocicletas.Add(Motocicleta);

                    if (db.SaveChanges() > 0)
                    {
                        //si todo se guardo correctamente y el usuario es el admin entonces crearle el usuario correspondiente
                        if (Request.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("Asistente")) && UserInDb == null)//si el usuario no existe en la bd de seguridad
                        {
                            var user = new ApplicationUser { UserName = motorizado.CorreoElectronico, Email = motorizado.CorreoElectronico, PhoneNumber = motorizado.Telefono, EmailConfirmed = true };

                            var result = await UserManager.CreateAsync(user, motorizado.Password);

                            if (result.Succeeded)//asignarlo al rol de motorizados
                                await UserManager.AddToRoleAsync(user.Id, "Motorizado");
                            else
                            {
                                db.Motocicletas.Remove(Motocicleta);
                                db.Motorizados.Remove(Motorizado);

                                AddErrors(result);
                                return View(motorizado);
                            }

                        }

                        ViewBag.Exito = true;

                        if (Request.IsAuthenticated)
                            return RedirectToAction("Index", "HomeUser");
                        else
                            return View(new MotorizadoViewModel());
                    }
                    else
                    {
                        db.Motorizados.Remove(Motorizado);
                        db.Motocicletas.Remove(Motocicleta);
                        ViewBag.Exito = false;

                        ModelState.AddModelError("", "Lo sentimos, ocurrio un error procesando su solicitud");
                    }
                }
            }
            else
                ModelState.AddModelError("", "Estimado usuario, verifique la información ingresada");

            return View(motorizado);
        }

        [HttpGet]
        public ActionResult AgregarMotocicleta(int Id)
        {
            var Motorizado = db.Motorizados.Find(Id);

            if (Motorizado == null)
            {
                return HttpNotFound();
            }

            var Motocicleta = new Motocicleta();
            Motocicleta.MotorizadoId = Motorizado.Id;

            ViewBag.NombreCompleto = Motorizado.NombreCompleto;
            return View(Motocicleta);
        }

        [HttpPost]
        public ActionResult AgregarMotocicleta(Motocicleta motocicleta)
        {
            var Motorizado = db.Motorizados.Find(motocicleta.MotorizadoId);
            ViewBag.NombreCompleto = Motorizado.NombreCompleto;
            ModelState.Remove("FechaDeValidez");

            if (ModelState.IsValid)
            {
                motocicleta.EstadoDeMotocicleta = true;
                motocicleta.FechaDeIngreso = DateTime.Now;
                motocicleta.FechaDeValidez = motocicleta.EsTemporal ? motocicleta.FechaDeValidez : DateTime.Parse("01/01/1900 00:00:00");

                db.Motocicletas.Add(motocicleta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(motocicleta);
        }
        // GET: Motorizados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.EstadoDeAfiliado = ListarEstadoAfiliacion();

            Motorizado motorizado = db.Motorizados.Find(id);

            if (motorizado == null)
            {
                return HttpNotFound();
            }
            return View(motorizado);
        }

        // POST: Motorizados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public ActionResult Edit([Bind(Include = "Id,EsAfiliado,EstadoDeAfiliado,FechaDeAfiliacion,EstadoDeMotorizado,CorreoElectronico,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Telefono,Foto,Sexo,Direccion,Cedula,FechaIngreso,CorreoElectronico,Cedula,VelocidadDeConexionId, DisponibilidadId")] Motorizado motorizado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(motorizado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", db.Motorizados.ToList());
            }
            return View(motorizado);
        }

        [HttpPost]
        public async Task<ActionResult> AfiliarMotorizado(int MotorizadoId)
        {
            try
            {
                var motorizado = db.Motorizados.Find(MotorizadoId);
                var estadoAfiliadoDefault = motorizado.EstadoDeAfiliado;

                if (motorizado == null)
                    return Json(new { exito = false, message = "Error: Estimado usuario debe seleccionar un motorizado válido" }, JsonRequestBehavior.AllowGet);
                else
                {
                    if (motorizado.EstadoDeAfiliado == (short)EstadoDeAfiliadoEnum.Afiliado)
                        return Json(new { exito = false, message = "Error: El motorizado seleccionado ya se encuentra afiliado" }, JsonRequestBehavior.AllowGet);

                    if (motorizado.EstadoDeAfiliado == (short)EstadoDeAfiliadoEnum.NoAplica)
                        return Json(new { exito = false, message = "Error: No se puede afiliar porque el motorizado seleccionado es empresarial" }, JsonRequestBehavior.AllowGet);

                    var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    var UserInDb = UserManager.FindByEmail(motorizado.CorreoElectronico);

                    if (UserInDb != null)
                        return Json(new { exito = false, message = "Ha ocurrido un error afiliando al motorizado porque ya existe un usuario con el correo del motorizado seleccionado" }, JsonRequestBehavior.AllowGet);
                    else
                    {
                        //afiliar al motorizado
                        motorizado.EstadoDeAfiliado = (short)EstadoDeAfiliadoEnum.Afiliado;
                        motorizado.FechaDeAfiliacion = DateTime.Now;
                        db.Entry(motorizado).State = EntityState.Modified;

                        if (db.SaveChanges() > 0)
                        {
                            var user = new ApplicationUser { UserName = motorizado.CorreoElectronico, Email = motorizado.CorreoElectronico, PhoneNumber = motorizado.Telefono, EmailConfirmed = true };
                            var defaultPassword = Utilidades.GenerateDefaultPasswordByEmail(motorizado.CorreoElectronico);

                            var result = await UserManager.CreateAsync(user, defaultPassword);

                            if (result.Succeeded)//asignarlo al rol de motorizados
                            {
                                var resultRole = await UserManager.AddToRoleAsync(user.Id, "Motorizado");

                                if (resultRole.Succeeded)
                                    return Json(new { exito = true, message = string.Format("Estimado usuario se ha afiliado con exito al motorizado, se le generaron las siguientes credenciales por defecto. Usuario: {0}  Contraseña: {1}", motorizado.CorreoElectronico, defaultPassword) }, JsonRequestBehavior.AllowGet);
                                else
                                {
                                    //revertir el estado del motorizado al estado que tenia anteriormente.
                                    motorizado.EstadoDeAfiliado = estadoAfiliadoDefault;
                                    motorizado.FechaDeAfiliacion = DateTime.Parse("01/01/1900 00:00:00");
                                    db.Entry(motorizado).State = EntityState.Modified;
                                    db.SaveChanges();
                                    UserManager.Delete(user);

                                    return Json(new { exito = false, message = "Ha ocurrido un error. " + string.Join(" | ", resultRole.Errors.ToList()) }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                //revertir el estado del motorizado al estado que tenia anteriormente.
                                motorizado.EstadoDeAfiliado = estadoAfiliadoDefault;
                                motorizado.FechaDeAfiliacion = DateTime.Parse("01/01/1900 00:00:00");
                                db.Entry(motorizado).State = EntityState.Modified;
                                db.SaveChanges();

                                return Json(new { exito = false, message = "Ha ocurrido un error./n" + string.Join(" | ", result.Errors.ToList()) }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                            return Json(new { exito = false, message = "Estimado usuario ha ocurrido un error afiliando al motorizado seleccionado" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { exito = false, message = "Estimado usuario ha ocurrido un error afiliando al motorizado seleccionado" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { exito = false, message = "Ha ocurrido un error procesando la solicitud!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RechazarAfiliacion(int MotorizadoId)
        {
            var motorizado = db.Motorizados.Find(MotorizadoId);
            var estadoAfiliadoDefault = motorizado.EstadoDeAfiliado;

            if (motorizado == null)
                return Json(new { exito = false, message = "Error: Estimado usuario debe seleccionar un motorizado válido" }, JsonRequestBehavior.AllowGet);
            else
            {
                if (motorizado.EstadoDeAfiliado == (short)EstadoDeAfiliadoEnum.Afiliado)
                    return Json(new { exito = false, message = "Error: No se puede rechazar la solicitud de afiliación porque el motorizado seleccionado ya se encuentra afiliado" }, JsonRequestBehavior.AllowGet);

                if (motorizado.EstadoDeAfiliado == (short)EstadoDeAfiliadoEnum.NoAplica)
                    return Json(new { exito = false, message = "Error: No se puede rechazar porque el motorizado seleccionado es empresarial" }, JsonRequestBehavior.AllowGet);
                
                if(motorizado.EstadoDeAfiliado == (short)EstadoDeAfiliadoEnum.Denegado)
                    return Json(new { exito = false, message = "Error: No se puede rechazar la solicitud de afiliación porque el motorizado seleccionado ya se encuentra rechazado" }, JsonRequestBehavior.AllowGet);

                //rechazar al motorizado
                motorizado.EstadoDeAfiliado = (short)EstadoDeAfiliadoEnum.Denegado;
                motorizado.FechaDeAfiliacion = DateTime.Parse("01/01/1900 00:00:00");
                motorizado.FechaRechazoAfiliacion = DateTime.Now;
                db.Entry(motorizado).State = EntityState.Modified;

                if(db.SaveChanges() > 0)
                    return Json(new { exito = true, message = "Estimado usuario se ha rechazado la solicitud de afiliación con exito" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { exito = false, message = "Estimado usuario ha ocurrido un error rechazando la solitud de afiliación del motorizado seleccionado" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MotocicletasDelMotorizado(int MotorizadoId)
        {
            var motorizado = db.Motorizados.Find(MotorizadoId);

            if (motorizado == null)//si no existe el motorizado entonces devolver una lista vacia
                return Json(new List<Motocicleta>(),JsonRequestBehavior.AllowGet);
            else
                return Json(JsonConvert.SerializeObject(_mapper.Map<ICollection<MotocicletaIndexViewModel>>(motorizado.Motocicletas.ToList())), JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<EstadoDeAfiliadoEnum> ListarEstadoAfiliacion()
        {
            var Estados = Enum.GetValues(typeof(EstadoDeAfiliadoEnum)).Cast<EstadoDeAfiliadoEnum>().ToList();

            if (Request.IsAuthenticated && User.IsInRole("Admin"))
                return Estados;
            else
                return new List<EstadoDeAfiliadoEnum>() { EstadoDeAfiliadoEnum.Solicitud };
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
