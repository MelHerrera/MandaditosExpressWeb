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
            Pagos = new HashSet<Pago>();
            Seguimientos = new HashSet<Seguimiento>();
            FechaDelEnvio = DateTime.Now;
            LugarOrigen = new Lugar();
            LugarDestino = new Lugar();
            this.TiposDeServicio = db.TiposDeServicio.ToList();
            this.Servicios = db.Servicios.ToList();
            TipoDeServicioId = -1;
            var gestion = db.TiposDeServicio.FirstOrDefault(x => x.DescripcionTipoDeServicio.ToUpper().Contains("Banc"));
            GestionBancariaId = gestion != null ? gestion.Id : -1;
            TiposDePago = db.TiposDePago.ToList();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        public string DescripcionDeEnvio { get; set; }

        public DateTime FechaDelEnvio { get; set; }

        public Lugar LugarOrigen { get; set; }

        public Lugar LugarDestino { get; set; }

        public double DistanciaEntregaRecep { get; set; }

        public string NombresDelReceptor { get; set; }

        public string ApellidosDelReceptor { get; set; }

        public string CedulaDelReceptor { get; set; }

        [Display(Name = "El ancho es menor a 40 Pulg?")]
        public bool Ancho { get; set; }

        [Display(Name = "El alto es menor a 40 Pulg?")]
        public bool Alto { get; set; }

        [Display(Name = "El peso es menor a 40 Libras?")]
        public bool Peso { get; set; }

        public Decimal MontoDeDinero { get; set; }

        public string TelefonoDelReceptor { get; set; }

        public DateTime FechaDeEntrega { get; set; }

        public bool EsUrgente { get; set; }

        public TimeSpan HoraDeEntrega { get; set; }

        public double PrecioDeRecargo { get; set; }

        public short EstadoDelEnvio { get; set; }

        public int MotocicletaId { get; set; }

        public int AsistenteId { get; set; }

        public int ClienteId { get; set; }

        public int MotorizadoId { get; set; }

        public int TipoDePagoId { get; set; }

        public int ServicioId { get; set; }

        public int TipoDeServicioId { get; set; }

        public int GestionBancariaId { get; set; }

        public List<TipoDeServicio> TiposDeServicio { get; set; }

        public List<Servicio> Servicios { get; set; }
        public virtual Motocicleta Motocicleta { get; set; }
        public virtual Asistente Asistente { get; set; }

        public virtual ICollection<TipoDePago> TiposDePago { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pago> Pagos { get; set; }
        public virtual Motorizado Motorizado { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seguimiento> Seguimientos { get; set; }
        public virtual Servicio Servicio { get; set; }
    }
}