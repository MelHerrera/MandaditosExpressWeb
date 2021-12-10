using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;
using MandaditosExpress.Models.ViewModels;
using Newtonsoft.Json;
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
        private IMapper _mapper;

        public ConsultasController(IMapper mapper)
        {
            _mapper = mapper;
        }

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

        [HttpGet]
        public ActionResult CreditosCliente()
        {
            ViewBag.ClienteId = new SelectList(_mapper.Map<ICollection<ClienteBusquedasViewModel>>(db.Clientes.ToList()), nameof(Cliente.Id), nameof(Cliente.NombreCompleto));

            var viewmodel = new EnviosCreditoConsultaViewModel();
            viewmodel.FechaDesde = DateTime.Now;
            viewmodel.FechaHasta = DateTime.Now;

            ViewBag.Data = JsonConvert.SerializeObject(viewmodel);
            return View(new List<EnviosCreditoViewModel>());
        }

        [HttpPost]
        public ActionResult CreditosCliente(DateTime FechaDesde, DateTime FechaHasta, int ClienteId)
        {
            ViewBag.ClienteId = new SelectList(_mapper.Map<ICollection<ClienteBusquedasViewModel>>(db.Clientes.ToList()), nameof(Cliente.Id), nameof(Cliente.NombreCompleto), ClienteId);

            var viewmodel = new EnviosCreditoConsultaViewModel();
            viewmodel.FechaDesde = FechaDesde;
            viewmodel.FechaHasta = FechaHasta;
            viewmodel.ClienteId = ClienteId;

            ViewBag.Data = JsonConvert.SerializeObject(viewmodel);

            //envios que se realizaron a un cliente en un periodo de fecha
            //los envios realizados deben tener estado diferente a solicitud o rechazado
            var envios = db.Envios.Where(it => it.EsAlCredito && it.FechaDelEnvio >= FechaDesde && it.FechaDelEnvio <= FechaHasta && it.ClienteId == ClienteId
            && (it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso || it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado)).ToList().OrderBy(it => it.FechaDelEnvio);


            var EnviosAlCredito = _mapper.Map<ICollection<EnviosCreditoViewModel>>(envios);
            return View(EnviosAlCredito);
        }

        [HttpGet]
        public ActionResult EnviosMensuales()
        {
            var primerDiaMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var envios = db.Envios.Where(it => it.FechaDelEnvio >= primerDiaMes && it.FechaDelEnvio <= DateTime.Now).GroupBy(it => it.FechaDelEnvio.Day).Select(x => new EnviosMensualesViewModel
            {
                Dia = x.Key,
                Total = x.Count(),//total por fecha del mes
                EnviosSolicitud = x.Where(y => y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Solicitud).Count(),
                EnviosProceso = x.Where(y => y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.EnProceso).Count(),
                EnviosFinalizado = x.Where(y => y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado).Count(),
                EnviosRechazado = x.Where(y => y.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Rechazado).Count()
            });

            int Total = 0;
            envios.ToList().Select(y => y.Total).ToList().ForEach(it => Total += it);

            ViewBag.EnviosTotal = Total;//Total en el mes
            return View(envios);
        }

        public DateTime MapDate(int day)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
        }
    }
}