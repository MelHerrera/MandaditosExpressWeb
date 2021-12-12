using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;
using MandaditosExpress.Models.Utileria;
using MandaditosExpress.Models.ViewModels;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MotorizadosController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

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
            var Disponibilidades = db.Disponibilidad.ToList();
            var Calidades = db.VelocidadDeConexion.ToList();

            if (Disponibilidades.Count <= 0)
            Disponibilidades.Insert(0, new Disponibilidad() {Id=-1, Descripcion ="--Sin Registros--" });

            if (Calidades.Count <= 0)
                Calidades.Insert(0, new CalidadDeConexion() { Id = -1, Descripcion = "--Sin Registros--" });

            ViewBag.DisponibilidadId = new SelectList(Disponibilidades,"Id", "Descripcion");
            ViewBag.VelocidadDeConexionId = new SelectList(Calidades, "Id", "Descripcion");

            return View(MotorizadoVM);
        }

        // POST: Motorizados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MotorizadoViewModel motorizado)
        {
            var Disponibilidades = db.Disponibilidad.ToList();
            var Calidades = db.VelocidadDeConexion.ToList();

            if (Disponibilidades.Count <= 0)
                Disponibilidades.Insert(0, new Disponibilidad() { Id = -1, Descripcion = "--Sin Registros--" });

            if (Calidades.Count <= 0)
                Calidades.Insert(0, new CalidadDeConexion() { Id = -1, Descripcion = "--Sin Registros--" });

            ViewBag.DisponibilidadId = new SelectList(Disponibilidades, "Id", "Descripcion", motorizado.DisponibilidadId);
            ViewBag.VelocidadDeConexionId = new SelectList(Calidades, "Id", "Descripcion", motorizado.VelocidadDeConexionId);


            if (ModelState.IsValid )
            {
                if (motorizado.DisponibilidadId > 0 && motorizado.VelocidadDeConexionId > 0)
                {

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
                        EsAfiliado = (Request.IsAuthenticated && User.IsInRole("Admin")) ? false : true,
                        EstadoDeAfiliado = (Request.IsAuthenticated && User.IsInRole("Admin")) ? (short)EstadoDeAfiliadoEnum.NoAplica : ((short)EstadoDeAfiliadoEnum.Solicitud),
                        VelocidadDeConexionId = motorizado.VelocidadDeConexionId,
                        DisponibilidadId = motorizado.DisponibilidadId,
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
                            ViewBag.Exito = true;
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
                    ModelState.AddModelError("", "La Disponibilidad y Calidad de conexión a internet es obligatoria");
            }
            else
                ModelState.AddModelError("", "Lo sentimos, ocurrio un error procesando su solicitud");

            return View(motorizado);
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
        public ActionResult Edit([Bind(Include = "Id,EsAfiliado,EstadoDeAfiliado,FechaDeAfiliacion,EstadoDeMotorizado,CorreoElectronico,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Telefono,Foto,Sexo,Direccion,Cedula,FechaIngreso,CorreoElectronico,Cedula,VelocidadDeConexionId, DisponibilidadId")] Motorizado motorizado)
        {
            if (ModelState.IsValid)
            {
                if (motorizado.EstadoDeAfiliado == (short)EstadoDeAfiliadoEnum.Afiliado)
                {
                    motorizado.FechaDeAfiliacion = DateTime.Now;
                    motorizado.EsAfiliado = true;
                }

                db.Entry(motorizado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(motorizado);
        }

        // GET: Motorizados/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Motorizados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Motorizado motorizado = db.Motorizados.Find(id);
            db.Motorizados.Remove(motorizado);
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

        public List<EstadoDeAfiliadoEnum> ListarEstadoAfiliacion()
        {
            var Estados = Enum.GetValues(typeof(EstadoDeAfiliadoEnum)).Cast<EstadoDeAfiliadoEnum>().ToList();

            if (Request.IsAuthenticated && User.IsInRole("Admin"))
                return Estados;
            else
                return new List<EstadoDeAfiliadoEnum>() { EstadoDeAfiliadoEnum.Solicitud };
        }
    }
}
