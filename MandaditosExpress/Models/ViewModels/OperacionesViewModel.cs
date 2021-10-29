using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models.ViewModels
{
    public class CotizacionViewModel
    {
        private MandaditosDB db;
        public CotizacionViewModel()
        {
            db = new MandaditosDB();
            FechaDeLaCotizacion = DateTime.Now;
            FechaDeValidez = DateTime.Now.AddDays(15);
            TiposDeServicios = db.TiposDeServicio.ToList();
            var gestion = db.TiposDeServicio.FirstOrDefault(x => x.DescripcionTipoDeServicio.ToUpper().Contains("Banc"));
            GestionBancariaId = gestion!=null ? gestion.Id : -1;
        }

        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name ="Descripción")]
        public string DescripcionDeCotizacion { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaDeLaCotizacion { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaDeValidez { get; set; }

        public Lugar LugarDeOrigen { get; set; }
        public Lugar LugarDeDestino { get; set; }

        public float DistanciaOrigenDestino { get; set; }

        [Required]
        public bool EsEspecial { get; set; }

        [Required]
        public decimal MontoTotal { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Tipo de Servicio")]
        public int TipoDeServicioId { get; set; }

        public decimal MontoDeDinero { get; set; }

        public int GestionBancariaId { get; set; }

        public List<TipoDeServicio> TiposDeServicios { get; set; }
    }

    public class EnvioViewModel
    {
        private MandaditosDB db = new MandaditosDB();
        public EnvioViewModel()
        {
            FechaDelEnvio = DateTime.Now;
            LugarOrigen = new Lugar();
            LugarDestino = new Lugar();
            TiposDeServicio = db.TiposDeServicio.ToList();
            Servicios = db.Servicios.ToList();
            TipoDeServicioId = -1;
            TiposDePago = db.TiposDePago.ToList();
            var gestion = db.TiposDeServicio.FirstOrDefault(x => x.DescripcionTipoDeServicio.ToUpper().Contains("Banc"));
            GestionBancariaId = gestion != null ? gestion.Id : -1;
        }

        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name ="Descripción")]
        public string DescripcionDeEnvio { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime FechaDelEnvio { get; set; }

        [Required]
        [Display(Name = "Origen")]
        public Lugar LugarOrigen { get; set; }

        [Required]
        [Display(Name = "Destino")]
        public Lugar LugarDestino { get; set; }

        [Required]
        [Display(Name = "Distancia")]
        public double DistanciaEntregaRecep { get; set; }

        [Required]
        [Display(Name = "Nombres y Apellidos del receptor")]
        public string NombresDelReceptor { get; set; }

        [Required]
        [Display(Name = "Cédula receptor")]
        public string CedulaDelReceptor { get; set; }

        [Display(Name = "¿El peso es menor a 50 Libras?")]
        public bool Peso { get; set; }

        [Required]
        [Display(Name = "Monto de dinero")]
        public Decimal MontoDeDinero { get; set; }

        [Required]
        public decimal MontoTotal { get; set; }

        [Required]
        [Display(Name = "Celular del receptor")]
        public string TelefonoDelReceptor { get; set; }

        public bool EsUrgente { get; set; }

        [Display(Name = "¿Es ida y regreso?")]
        public bool DebeRegresarATienda { get; set; }

        [Display(Name = "¿El motorizado recibirá algun dinero?")]
        public bool DebeRecibirDinero { get; set; }

        [Display(Name = "¿Cuanto recibirá?")]
        public decimal MontoARecibir { get; set; }

        [Display(Name = "¿El motorizado necesita cambio?")]
        public bool Cambio { get; set; }

        [Display(Name = "¿Cuanto de cambio?")]
        public decimal MontoCambio { get; set; }

        public double PrecioDeRecargo { get; set; }

        public short EstadoDelEnvio { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Método de pago")]
        public int TipoDePagoId { get; set; }

        [Display(Name = "Cotización")]
        public int CotizacionId { get; set; }

        [Required]
        [Display(Name ="Servicio")]
        public int ServicioId { get; set; }

        [Required]
        [Display(Name = "Tipo de servicio")]
        public int TipoDeServicioId { get; set; }

        public int GestionBancariaId { get; set; }//propiedad usada solo para validaciones

        public List<TipoDeServicio> TiposDeServicio { get; set; }

        public List<Servicio> Servicios { get; set; }

        public virtual ICollection<TipoDePago> TiposDePago { get; set; }
    }
}