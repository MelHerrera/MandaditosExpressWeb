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
    [Authorize(Roles ="Admin, Cliente")]
    public class HomeUserController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: HomeUser
        public ActionResult Index()
        {
            var UserName = Request.GetOwinContext().Authentication.User.Identity.Name;
            ViewBag.PersonaActual = new Utileria().BuscarPersonaPorUsuario(UserName);
            ViewBag.Rol = new Utileria().GetRolesDeUsuario(UserName);

            var IndexData = new HomeUserViewModel();
            IndexData.EnviosDelDia = getEnviosDia();
            IndexData.EnviosMensuales = getEnviosMensuales();
            IndexData.EnviosAnuales = getEnviosAnuales();

            ViewBag.IndexHomeUserData = JsonConvert.SerializeObject(IndexData);
            return View();
        }

        public int getEnviosMensuales()
        {
            var PrimerDiaDelMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month,1);
            var envios = db.Envios.Where(it=> it.FechaDelEnvio >= PrimerDiaDelMes && it.FechaDelEnvio<= DateTime.Now).ToList();
            return envios.Count;
        }

        public int getEnviosAnuales()
        {
            var envios = db.Envios.Where(it=> it.FechaDelEnvio.Year == DateTime.Today.Year).ToList();
            return envios.Count;
        }

        public int getEnviosDia()
        {
            var envios = db.Envios.Where(it=> it.FechaDelEnvio >= DateTime.Today && it.FechaDelEnvio <= DateTime.Now).ToList();
            return envios.Count;
        }
    }
}