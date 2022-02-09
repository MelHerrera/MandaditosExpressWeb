using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using MandaditosExpress.Models.Enum;

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

        public LugarViewModel LugarOrigen { get; set; }
        public LugarViewModel LugarDestino { get; set; }

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

        public List<TipoDeServicioViewModel> TiposDeServicios { get; set; }
    }

    public class IndexCotizacionViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Descripción")]
        public string DescripcionDeCotizacion { get; set; }

        public string CodigoDeCotizacion { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaDeLaCotizacion { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaDeValidez { get; set; }

        public string LugarDestino { get; set; }

        public float DistanciaOrigenDestino { get; set; }
        public decimal MontoDeDinero { get; set; }

        [Required]
        public bool EsEspecial { get; set; }

        [Required]
        public decimal MontoTotal { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Tipo de Servicio")]
        public int TipoDeServicioId { get; set; }

    }
    public class SolicitudEnvioViewModel
    {

        public SolicitudEnvioViewModel()
        {
            FechaDelEnvio = DateTime.Now;
            LugarOrigen = new LugarViewModel();
            LugarDestino = new LugarViewModel();
            Servicio = new ServicioViewModel();
            TipoDeServicioId = -1;
            Peso = true; // poner por defecto que el peso es menor a 50 libras
            EstadoDelEnvio = (short)EstadoDelEnvioEnum.Solicitud;
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
        public LugarViewModel LugarOrigen { get; set; }

        [Required]
        [Display(Name = "Destino")]
        public LugarViewModel LugarDestino { get; set; }

        [Required]
        [Display(Name = "Distancia")]
        public float DistanciaEntregaRecep { get; set; }

        [Required]
        [Display(Name = "Nombres y Apellidos del receptor")]
        public string NombresDelReceptor { get; set; }

        [Display(Name = "Cédula receptor")]
        public string CedulaDelReceptor { get; set; }

        [Display(Name = "¿El peso es menor a 50 Libras?")]
        public bool Peso { get; set; }

        [Display(Name = "Monto de la gestión")]
        public decimal MontoDeDinero { get; set; }

        [Required]
        [Display(Name = "Monto total del envio")]
        public decimal MontoTotalDelEnvio { get; set; }

        [Required]
        [Display(Name = "Celular del receptor")]
        [MinLength(8)]
        [MaxLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string TelefonoDelReceptor { get; set; }

        [Display(Name = "¿Urgente?")]
        public bool EsUrgente { get; set; }

        [Display(Name = "¿Es ida y regreso?")]
        public bool DebeRegresarATienda { get; set; }

        [Display(Name = "¿El motorizado recibirá algun dinero en efectivo?")]
        public bool DebeRecibirDinero { get; set; }

        [Display(Name = "¿Cuanto recibirá?")]
        public decimal MontoARecibir { get; set; }

        [Display(Name = "¿El motorizado necesita cambio?")]
        public bool DebeRecibirCambio { get; set; }

        [Display(Name = "¿Cuanto de cambio?")]
        public decimal MontoCambio { get; set; }

        [Display(Name = "¿Estado?")]
        public short EstadoDelEnvio { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Método de pago")]
        public int TipoDePagoId { get; set; }

        [Display(Name = "Cotización")]
        public int? CotizacionId { get; set; }

        public string CodigoDeLaCotizacion { get; set; }

        public int? CreditoId { get; set; }

        [Display(Name ="Servicio")]
        public int? ServicioId { get; set; }

        [Required]
        [Display(Name = "Tipo de servicio")]
        public int TipoDeServicioId { get; set; }

        public bool TieneCredito { get; set; }

        public bool EsAlCredito { get; set; }

        public int GestionBancariaId { get; set; }//propiedad usada solo para validaciones

        public List<TipoDeServicioViewModel> TiposDeServicio { get; set; }

        public List<ServicioViewModel> Servicios { get; set; }

        public virtual ICollection<TipoDePagoViewModel> TiposDePago { get; set; }

        public virtual ServicioViewModel Servicio { get; set; }

    }

    public class AsignarMotorizadoViewModel
    {
        public EnvioViewModel Envio { get; set; }
        public ICollection<AsignacionMotorizadoViewModel> Motorizados { get; set; }
    }

    public class EnvioViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Descripción")]
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
        public float DistanciaEntregaRecep { get; set; }

        [Required]
        [Display(Name = "Nombres y Apellidos del receptor")]
        public string NombresDelReceptor { get; set; }

        [Required]
        [Display(Name = "Cédula receptor")]
        public string CedulaDelReceptor { get; set; }

        [Display(Name = "¿El peso es menor a 50 Libras?")]
        public bool Peso { get; set; }

        [Display(Name = "Monto de la gestión")]
        public decimal MontoDeDinero { get; set; }

        [Required]
        [Display(Name = "Monto total del envio")]
        public decimal MontoTotalDelEnvio { get; set; }

        [Required]
        [Display(Name = "Celular del receptor")]
        [MinLength(8)]
        [MaxLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string TelefonoDelReceptor { get; set; }

        [Display(Name = "¿Urgente?")]
        public bool EsUrgente { get; set; }

        [Display(Name = "¿Es ida y regreso?")]
        public bool DebeRegresarATienda { get; set; }

        [Display(Name = "¿El motorizado recibirá algun dinero?")]
        public bool DebeRecibirDinero { get; set; }

        [Display(Name = "¿Cuanto recibirá?")]
        public decimal MontoARecibir { get; set; }

        [Display(Name = "¿El motorizado necesita cambio?")]
        public bool DebeRecibirCambio { get; set; }

        [Display(Name = "¿Cuanto de cambio?")]
        public decimal MontoCambio { get; set; }

        [Display(Name = "¿Estado?")]
        public short EstadoDelEnvio { get; set; }

        public string EstadoDelEnvioClass { get; set; }

        public string EstadoDelEnvioDescripcion { get; set; }

        public int ClienteId { get; set; }

        public string ClienteNombres { get; set; }

        public byte[] ClienteFoto { get; set; }

        [Required]
        [Display(Name = "Método de pago")]
        public int TipoDePagoId { get; set; }

        [Display(Name = "Cotización")]
        public int? CotizacionId { get; set; }

        public string CotizacionDescripcion { get; set; }

        [Required]
        [Display(Name = "Descripción breve del Servicio")]
        public int ServicioId { get; set; }

        [Required]
        [Display(Name = "Tipo de servicio")]
        public int TipoDeServicioId { get; set; }
        public string TipoDeServicioDescripcion { get; set; }

        public int? MotorizadoId { get; set; }

        public string MotorizadoNombres { get; set; }
    }

    public class EnvioPagoViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string CodigoDeEnvio { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Descripción")]
        public string DescripcionDeEnvio { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime FechaDelEnvio { get; set; }

        [Required]
        [Display(Name = "Monto total del envio")]
        public decimal MontoTotalDelEnvio { get; set; }

        public double DistanciaEntregaRecep { get; set; }

        public int ClienteId { get; set; }

    }

    public class IndexEnvioViewModel
    {

        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string CodigoDeEnvio { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Descripción del envío")]
        public string DescripcionDeEnvio { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime FechaDelEnvio { get; set; }

        [Required]
        [Display(Name = "Distancia")]
        public double DistanciaEntregaRecep { get; set; }

        [Required]
        [Display(Name = "Nombres del receptor")]
        public string NombresDelReceptor { get; set; }

        [Display(Name = "Cédula receptor")]
        public string CedulaDelReceptor { get; set; }

        [Display(Name = "El peso es menor a 50 Libras?")]
        public bool Peso { get; set; }

        [Required]
        [Display(Name = "Monto de la gestión")]
        public decimal MontoDeDinero { get; set; }

        [Required]
        [Display(Name = "Monto total del envio")]
        public decimal MontoTotalDelEnvio { get; set; }

        [Required]
        [Display(Name = "Celular del receptor")]
        [MinLength(8)]
        [MaxLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string TelefonoDelReceptor { get; set; }

        [Display(Name = "¿Urgente?")]
        public bool EsUrgente { get; set; }

        [Display(Name = "¿Es ida y regreso?")]
        public bool DebeRegresarATienda { get; set; }

        [Display(Name = "¿El motorizado recibirá algun dinero?")]
        public bool DebeRecibirDinero { get; set; }

        [Display(Name = "¿Cuanto recibirá?")]
        public decimal MontoARecibir { get; set; }
        [Display(Name = "¿Cuanto de cambio?")]
        public decimal MontoCambio { get; set; }

        [Display(Name = "¿El motorizado necesita cambio?")]
        public bool DebeRecibirCambio { get; set; }

        //[Display(Name = "Estado")]
        //public short EstadoDelEnvio { get; set; }
        public string EstadoDelEnvioClass { get; set; }
        public string EstadoDelEnvioText { get; set; }

        public bool Finalizado { get; set; }
        public bool Rechazado { get; set; }

        [Display(Name = "Al crédito")]
        public bool EsAlCredito { get; set; }
        public bool EstaRetrasado { get; set; }

        public double TiempoRetraso { get; set; }

        [StringLength(250)]
        public string MotivoDeRechazo { get; set; }
        public string Cliente { get; set; }

        public virtual Lugar LugarOrigen { get; set; }
        public virtual Lugar LugarDestino { get; set; }

        public bool Asignado { get; set; }
        public string NombresDelMotorizado { get; set; }
    }

    public class EnvioHistorialViewModel
    {
        public string DescripcionDeEnvio { get; set; }
        public int TiempoTranscurrido { get; set; }
    }
}