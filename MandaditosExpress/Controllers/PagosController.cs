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
using MandaditosExpress.Models.Enum;
using MandaditosExpress.Models.ViewModels;
using Newtonsoft.Json;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PagosController : Controller
    {
        private MandaditosDB db;
        private readonly IMapper _mapper;

        public PagosController(IMapper mapper)
        {
            db = new MandaditosDB();
            _mapper = mapper;
        }

        // GET: Pagos
        public ActionResult Index()
        {
            var pagos = db.Pagos.Include(p => p.Credito).Include(p => p.Envio).Include(p => p.Moneda).Include(p => p.TipoDePago);
            return View(pagos.ToList());
        }

        // GET: Pagos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = db.Pagos.Find(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            return View(pago);
        }

        // GET: Pagos/Create
        public ActionResult Create()
        {
            var PagoViewModel = new PagoViewModel();

            PagoViewModel.Monedas = _mapper.Map<ICollection<MonedaViewModel>>(db.Monedas).ToList();
            PagoViewModel.TiposDePago = _mapper.Map<ICollection<TipoDePagoViewModel>>(db.TiposDePago).ToList();
            PagoViewModel.Clientes = _mapper.Map<ICollection<ClientePagoViewModel>>(db.Clientes).ToList();

            return View(PagoViewModel);
        }

        // POST: Pagos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NumeroDePago,FechaDePago,MontoADelPago,Cambio,CambioDolar,MonedaId,TipoDePagoId,EnvioId,CreditoId,EstadoDelPago")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Pagos.Add(pago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreditoId = new SelectList(db.Creditos, "Id", "Id", pago.CreditoId);
            ViewBag.EnvioId = new SelectList(db.Envios, "Id", "DescripcionDeEnvio", pago.EnvioId);
            ViewBag.MonedaId = new SelectList(db.Monedas, "Id", "NombreDeMoneda", pago.MonedaId);
            ViewBag.TipoDePagoId = new SelectList(db.TiposDePago, "Id", "Descripcion", pago.TipoDePagoId);
            return View(pago);
        }

        // GET: Pagos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = db.Pagos.Find(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreditoId = new SelectList(db.Creditos, "Id", "Id", pago.CreditoId);
            ViewBag.EnvioId = new SelectList(db.Envios, "Id", "DescripcionDeEnvio", pago.EnvioId);
            ViewBag.MonedaId = new SelectList(db.Monedas, "Id", "NombreDeMoneda", pago.MonedaId);
            ViewBag.TipoDePagoId = new SelectList(db.TiposDePago, "Id", "Descripcion", pago.TipoDePagoId);
            return View(pago);
        }

        // POST: Pagos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NumeroDePago,FechaDePago,MontoADelPago,Cambio,CambioDolar,MonedaId,TipoDePagoId,EnvioId,CreditoId,EstadoDelPago")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreditoId = new SelectList(db.Creditos, "Id", "Id", pago.CreditoId);
            ViewBag.EnvioId = new SelectList(db.Envios, "Id", "DescripcionDeEnvio", pago.EnvioId);
            ViewBag.MonedaId = new SelectList(db.Monedas, "Id", "NombreDeMoneda", pago.MonedaId);
            ViewBag.TipoDePagoId = new SelectList(db.TiposDePago, "Id", "Descripcion", pago.TipoDePagoId);
            return View(pago);
        }

        // GET: Pagos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = db.Pagos.Find(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pago pago = db.Pagos.Find(id);
            db.Pagos.Remove(pago);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult FiltrarEnvios(int ClienteId)
        {
            if (ClienteId > 0)
            {
                //lista de envios del cliente seleccionado que no son al credito y que aun no se hayan pagado y que esten en estado en proceso o realizados
                //TODO descomentar esta validacion para produccion
                //var ListEnvios = db.Envios.Where(it => it.ClienteId == ClienteId && !it.EsAlCredito && it.Pagos.Count <=0 
                //&& ( it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso || it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado )).ToList();

                //comentar esta lista de envios para produccion, ya que, no verifica el estado del envio
                var ListEnvios = db.Envios.Where(it => it.ClienteId == ClienteId && !it.EsAlCredito && it.Pagos.Count <= 0).ToList();
                var Envios = _mapper.Map<ICollection<EnvioPagoViewModel>>(ListEnvios).ToList();
               
                //JSONConvert dont put datetime format like  /Date(1639415480000)/
                return Json(new { exito = true, data = JsonConvert.SerializeObject(Envios) }, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FiltrarCreditos(int ClienteId)
        {

            if (ClienteId > 0)
            {
                var DefaultFecha = DateTime.Parse("01/01/1900");
                //por defecto la fecha de cancelacion es 1900, significa que si aun tiene esa fecha es porque no se ha pagado
                //TODO verificar que el credito tenga al menos 1 envio al credito asociado.
                var creditos = db.Creditos.Where(it => it.ClienteId == ClienteId && it.EstadoDelCredito
                && it.FechaDeInicio <= DateTime.Now && it.FechaDeCancelacion == DefaultFecha).ToList();

                //TODO validar que solo regrese los creditos que el monto es mayor a 0
                List<CreditoViewModel> Creditos = _mapper.Map<ICollection<CreditoViewModel>>(creditos).ToList();
                //JSONConvert dont put datetime format like  /Date(1639415480000)/	
                return Json(new { exito = true, data = JsonConvert.SerializeObject(Creditos) }, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
         
        [HttpGet]
        public JsonResult CalcularMontoCredito(ICollection<int> CreditosId)
        {
            if (CreditosId.Count > 0)
            {
                var MontoTotal = 0.0M;

                foreach (var item in CreditosId)
                {
                    var Credito = db.Creditos.Find(item);
                    //sacar todos los envios que:
                    // 1. Han sido por medio del metodo de pago credito
                    // 2. No hayan sido pagados
                    // 3. La fecha del envio se encuentre entre el rango de la fecha de inicio y vencimiento del credito, asi, se estarian pagando todos los envios en el periodo del credito
                    var envios = db.Envios.Where(it => it.EsAlCredito && it.Pagos.Count <= 0 && it.FechaDelEnvio >= Credito.FechaDeInicio 
                    && it.FechaDelEnvio <= Credito.FechaDeVencimiento && it.CreditoId == Credito.Id ).ToList();

                    if (envios.Count() > 0)
                        envios.ForEach(it => MontoTotal += it.MontoTotalDelEnvio);
                }
                
                return Json(new { exito = true, data = MontoTotal }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { exito = false, message = "Ha ocurrido un error procesando tu solicitud" }, JsonRequestBehavior.AllowGet);
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
