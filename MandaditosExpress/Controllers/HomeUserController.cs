using AutoMapper;
using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;
using MandaditosExpress.Models.Utileria;
using MandaditosExpress.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MandaditosExpress.Controllers
{
    [Authorize(Roles = "Admin, Cliente")]
    public class HomeUserController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        private IMapper _mapper;

        public HomeUserController(IMapper mapper)
        {
            _mapper = mapper;
        }
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
            IndexData.TodosEnvios = User.IsInRole("Admin") ? getTodosEnvio() : User.IsInRole("Cliente") ? getTodosEnvio(PersonaActual.Id) : 0;
            IndexData.CreditosPendientes = User.IsInRole("Admin") ? CreditosPendientes() : User.IsInRole("Cliente") ? CreditosPendientes(PersonaActual.Id) : 0;
            IndexData.EnviosHistorial = User.IsInRole("Admin") ? _mapper.Map<ICollection<EnvioHistorialViewModel>>(GetLastFiveEnvios()) : _mapper.Map<ICollection<EnvioHistorialViewModel>>(GetLastFiveEnvios(PersonaActual.Id));
            IndexData.EnviosRechazados = User.IsInRole("Admin") ? GetEnvioRechazados() : User.IsInRole("Cliente") ? GetEnvioRechazados(PersonaActual.Id) : 0;
            IndexData.EnviosRealizados = User.IsInRole("Admin") ? GetEnvioRealizados() : User.IsInRole("Cliente") ? GetEnvioRealizados(PersonaActual.Id) : 0;

            ViewBag.EnviosSemana = User.IsInRole("Admin") ? GetEnviosSemana() : User.IsInRole("Cliente") ? GetEnviosSemana(PersonaActual.Id) : new List<int>();
            ViewBag.IndexHomeUserData = JsonConvert.SerializeObject(IndexData);
            return View();
        }

        public int GetEnvioRealizados()
        {
            return db.Envios.Where(it => it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado).ToList().Count;
        }
        public int GetEnvioRealizados(int ClienteId)
        {
            return db.Envios.Where(it => it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Realizado && it.ClienteId == ClienteId).ToList().Count;
        }
        public int GetEnvioRechazados()
        {
            return db.Envios.Where(it => it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Rechazado).ToList().Count;
        }
        public int GetEnvioRechazados(int ClienteId)
        {
            return db.Envios.Where(it => it.EstadoDelEnvio == (short)EstadoDelEnvioEnum.Rechazado && it.ClienteId == ClienteId).ToList().Count;
        }
        public int getTodosEnvio()
        {
            return db.Envios.ToList().Count;
        }

        public int getTodosEnvio(int ClienteId)
        {
            return db.Envios.Where(it => it.ClienteId == ClienteId).ToList().Count;
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

        public ICollection<EnvioHistorialViewModel> GetLastFiveEnvios()
        {
            var envios = db.Envios.OrderByDescending(it => it.FechaDelEnvio).Take(5).ToList();

            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<Envio, EnvioHistorialViewModel>().ForMember(x => x.TiempoTranscurrido, x => x.MapFrom(y => (DateTime.Now - y.FechaDelEnvio).TotalMinutes)));

            var MExplicitMap = new Mapper(config);
            var enviosMapped = MExplicitMap.Map<ICollection<EnvioHistorialViewModel>>(envios);

            return enviosMapped;
        }
        public ICollection<EnvioHistorialViewModel> GetLastFiveEnvios(int ClienteId)
        {
            var envios = db.Envios.Where(it => it.ClienteId == ClienteId).OrderByDescending(it => it.FechaDelEnvio).Take(5).ToList();

            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<Envio, EnvioHistorialViewModel>().ForMember(x => x.TiempoTranscurrido, x => x.MapFrom(y => (int)(DateTime.Now - y.FechaDelEnvio).TotalMinutes)));

            var MExplicitMap = new Mapper(config);
            var enviosMapped = MExplicitMap.Map<ICollection<EnvioHistorialViewModel>>(envios);

            return enviosMapped;
        }
        public List<int> GetEnviosSemana()
        {
            int diasTrasncurridos = DayOfWeek.Monday - DateTime.Today.DayOfWeek;
            DateTime firstDayWeek = diasTrasncurridos <= 0 ? DateTime.Today.AddDays(diasTrasncurridos) : DateTime.Today.AddDays(-6);

            var envios1 = db.Envios.Where(x => x.FechaDelEnvio >= firstDayWeek && x.FechaDelEnvio <= DateTime.Now).GroupBy(x => x.FechaDelEnvio.Day)
            .Select(x => new { day = x.Key, cant = x.Count() });

            var enviosLastWeek = new List<int>();

            for (var date = firstDayWeek; date <= DateTime.Now; date = date.AddDays(1))
            {
                var CantByDay = envios1.FirstOrDefault(it => it.day == date.Day && it.cant > 0);

                var cantidad = CantByDay != null ? CantByDay.cant > 0 ? CantByDay.cant : 0 : 0;
                enviosLastWeek.Add(cantidad);
            }

            return enviosLastWeek.ToList();
        }
        public List<int> GetEnviosSemana(int ClienteId)
        {
            int diasTrasncurridos = DayOfWeek.Monday - DateTime.Today.DayOfWeek;
            DateTime firstDayWeek = diasTrasncurridos <= 0 ? DateTime.Today.AddDays(diasTrasncurridos) : DateTime.Today.AddDays(-6);

            var envios1 = db.Envios.Where(x => x.FechaDelEnvio >= firstDayWeek && x.FechaDelEnvio <= DateTime.Now && x.ClienteId == ClienteId).GroupBy(x => x.FechaDelEnvio.Day)
            .Select(x => new { day = x.Key, cant = x.Count() });

            var enviosLastWeek = new List<int>();

            for (var date = firstDayWeek; date <= DateTime.Now; date = date.AddDays(1))
            {
                var CantByDay = envios1.FirstOrDefault(it => it.day == date.Day && it.cant > 0);

                var cantidad = CantByDay != null ? CantByDay.cant > 0 ? CantByDay.cant : 0 : 0;
                enviosLastWeek.Add(cantidad);
            }

            return enviosLastWeek.ToList();
        }
    }
}