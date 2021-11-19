using MandaditosExpress.Models;
using MandaditosExpress.Models.Utileria;
using MandaditosExpress.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin, Cliente")]
    public class HomeUserController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: HomeUser
        public ActionResult Index()
        {
            var UserName = Request.GetOwinContext().Authentication.User.Identity.Name;
            var PersonaActual = new Utileria().BuscarPersonaPorUsuario(UserName);
            ViewBag.PersonaActual = PersonaActual;
            ViewBag.Rol = new Utileria().GetRolesDeUsuario(UserName);

            var IndexData = new HomeUserViewModel();
            IndexData.EnviosDelDia = User.IsInRole("Admin") ? getEnviosDia() : User.IsInRole("Cliente") ? getEnviosDia(PersonaActual.Id) : 0;
            IndexData.EnviosMensuales = User.IsInRole("Admin") ? getEnviosMensuales() : User.IsInRole("Cliente") ? getEnviosMensuales(PersonaActual.Id) : 0;
            IndexData.EnviosAnuales = User.IsInRole("Admin") ? getEnviosAnuales() : User.IsInRole("Cliente") ? getEnviosAnuales(PersonaActual.Id) : 0;
            IndexData.CreditosPendientes = User.IsInRole("Admin") ? CreditosPendientes() : User.IsInRole("Cliente") ? CreditosPendientes(PersonaActual.Id) : 0;

            ViewBag.IndexHomeUserData = JsonConvert.SerializeObject(IndexData);
            return View();
        }

        public int getEnviosMensuales()
        {
            var PrimerDiaDelMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var envios = db.Envios.Where(it => it.FechaDelEnvio >= PrimerDiaDelMes && it.FechaDelEnvio <= DateTime.Now).ToList();
            return envios.Count;
        }

        public int getEnviosMensuales(int ClienteId)
        {
            var PrimerDiaDelMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var envios = db.Envios.Where(it => it.FechaDelEnvio >= PrimerDiaDelMes && it.FechaDelEnvio <= DateTime.Now && it.ClienteId == ClienteId).ToList();
            return envios.Count;
        }

        public int getEnviosAnuales()
        {
            var envios = db.Envios.Where(it => it.FechaDelEnvio.Year == DateTime.Today.Year).ToList();
            return envios.Count;
        }
        public int getEnviosAnuales(int ClienteId)
        {
            var envios = db.Envios.Where(it => it.FechaDelEnvio.Year == DateTime.Today.Year && it.ClienteId == ClienteId).ToList();
            return envios.Count;
        }

        public int getEnviosDia()
        {
            var envios = db.Envios.Where(it => it.FechaDelEnvio >= DateTime.Today && it.FechaDelEnvio <= DateTime.Now).ToList();
            return envios.Count;
        }
        public int getEnviosDia(int ClienteId)
        {
            var envios = db.Envios.Where(it => it.FechaDelEnvio >= DateTime.Today && it.FechaDelEnvio <= DateTime.Now && it.ClienteId == ClienteId).ToList();
            return envios.Count;
        }

        public int CreditosPendientes()
        {
            var defaultCancelacionDate = DateTime.Parse("01/01/1900");
            var Creditos = db.Creditos.Where(it => it.FechaDeCancelacion == defaultCancelacionDate).ToList();
            return Creditos.Count;
        }
        public int CreditosPendientes(int ClienteId)
        {
            var defaultCancelacionDate = DateTime.Parse("01/01/1900");
            var Creditos = db.Creditos.Where(it => it.FechaDeCancelacion == defaultCancelacionDate && it.ClienteId == ClienteId).ToList();
            return Creditos.Count;
        }
    }
}