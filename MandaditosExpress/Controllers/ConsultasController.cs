using MandaditosExpress.Models;
using MandaditosExpress.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MandaditosExpress.Controllers
{
    [Authorize]
    public class ConsultasController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        // GET: Consultas
        public ActionResult BuscarClientes()
        {
            ViewBag.FechaDesde = new DateTime();
            ViewBag.FechaHasta = new DateTime();
            return View(new List<Cliente>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuscarClientes(DateTime FechaDesde, DateTime FechaHasta)
        {
            ViewBag.FechaDesde = FechaDesde;
            ViewBag.FechaHasta = FechaHasta;

            var clientes = db.Clientes.Where(x => x.FechaIngreso >= FechaDesde && x.FechaIngreso <= FechaHasta);
                return View(clientes);
        }
    }
}