using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models.ViewModels
{
    public class TipoDeServicioViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        [Display(Name = "Descripción")]
        public string DescripcionTipoDeServicio { get; set; }

        [Display(Name = "Estado")]
        public bool EstadoTipoDeServicio { get; set; }
    }

    public class ServicioViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        [Display(Name = "Descripción")]
        public string DescripcionDelServicio { get; set; }

        public bool Estado { get; set; }

        [Required]
        [Display(Name = "Tipo de Servicio")]
        public int TipoDeServicioId { get; set; }

        public string DescripcionTipoDeServicio { get; set; }

    }

    public class TipoDePagoViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool EstadoTipoDePago { get; set; }

    }

    public class LugarViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public string Direccion { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }
    }

    public class PagoViewModel
    {
        public PagoViewModel()
        {
            Envios = new HashSet<EnvioPagoViewModel>();
            Creditos = new HashSet<CreditoViewModel>();
        }

        [Required]
        [Display(Name = "Monto a pagar")]
        public double MontoDelPago { get; set; }

        [Display(Name = "Cambio de la Moneda")]
        [Required]
        public double Cambio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Moneda")]
        public int MonedaId { get; set; }

        [Required]
        [Display(Name = "Método de Pago")]
        public int TipoDePagoId { get; set; }

        public virtual ICollection<MonedaViewModel> Monedas { get; set; }
        public virtual ICollection<TipoDePagoViewModel> TiposDePago { get; set; }
        public virtual ICollection<ClientePagoViewModel> Clientes { get; set; }
        public virtual ICollection<EnvioPagoViewModel> Envios { get; set; }
        public virtual ICollection<CreditoViewModel> Creditos { get; set; }
        public virtual ICollection<int> CreditosId { get; set; }//los id de los creditos que se pagaran
        public virtual ICollection<int> EnviosId { get; set; }//los id de los envios que se pagaran
    }
    public class IndexPagoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Número De Pago")]
        [MaxLength(30)]
        public string NumeroDePago { get; set; }

        [Required]
        [Display(Name = "Fecha De Pago")]
        public DateTime FechaDePago { get; set; }

        [Required]
        [Display(Name = "Monto")]
        public double MontoDelPago { get; set; }

        [Display(Name = "Cambio M.")]
        public double Cambio { get; set; }

        [Required]
        [Display(Name = "Moneda")]
        public string MonedaDescripcion { get; set; }

        [Required]
        [Display(Name = "Método de Pago")]
        public string TipoDePagoDescripcion { get; set; }

        [Display(Name = "Código de Envio")]
        public string EnvioCodigo { get; set; }

        [Display(Name = "Código de Crédito")]
        public string CreditoCodigo { get; set; }

        [Required]
        [Display(Name = "Concepto")]
        public string ConceptoDelPago { get; set; }
        public string ConceptoDelPagoClass { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public string ClienteNombres { get; set; }

    }
    public class MonedaViewModel
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre De La Moneda")]
        public string NombreDeMoneda { get; set; }

        [Required]
        public string Abreviatura { get; set; }

        public bool Estado { get; set; }
    }

    public class CreditoViewModel
    {

        public CreditoViewModel()
        {
            FechaDeCancelacion = DateTime.Parse("01/01/1900");
            CodigoDelCredito = "-";
        }

        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        [Display(Name = "Código")]
        public string CodigoDelCredito { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha De Inicio")]
        public DateTime FechaDeInicio { get; set; }

        [Display(Name = "Fecha De Vencimiento")]
        public DateTime FechaDeVencimiento { get; set; }

        [Display(Name = "Estado")]
        public bool EstadoDelCredito { get; set; }

        [Display(Name = "Fecha De Cancelacion")]
        public DateTime FechaDeCancelacion { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        public double MontoDelCredito { get; set; }

        public string NombreCompletoCliente { get; set; }
    }

    public class MotocicletaIndexViewModel
    {
        public MotocicletaIndexViewModel()
        {
            Color = "#3399FF";
            FechaDeValidez = DateTime.Parse("01/01/1900 00:00:00");
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Placa { get; set; }

        [Required]
        [MaxLength(7)]
        public string Color { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El campo Año es obligatorio.")]
        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Required]
        [Display(Name = "Propia")]
        public bool EsPropia { get; set; }

        [Required]
        public int Kilometraje { get; set; }
        public DateTime FechaDeIngreso { get; set; }

        [Required]
        [Display(Name = "Temporal")]
        public bool EsTemporal { get; set; }

        [Display(Name = "Fecha De Validez")]
        public DateTime FechaDeValidez { get; set; }
        public int MotorizadoId { get; set; }
        public bool EstadoDeMotocicleta { get; set; }
    }

    public class CostoIndexViewModel
    {
        public CostoIndexViewModel()
        {
            FechaDeInicio = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaDeInicio { get; set; }

        [Required]
        [Display(Name = "Fecha de Fin")]
        [DataType(DataType.Date)]
        public DateTime FechaDeFin { get; set; }

        [Required]
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Display(Name = "Costo por Gasolina (CG)")]
        public float CostoDeGasolina { get; set; }

        [Display(Name = "Costo por Asistencia (CA)")]
        public float CostoDeAsistencia { get; set; }

        [Display(Name = "Costo por Motorizado (CM)")]
        public float CostoDeMotorizado { get; set; }

        [Required]
        [Display(Name = "Distancia Base en Km (DB)")]
        public float DistanciaBase { get; set; }

        [Required]
        [Display(Name = "Precio por Km en C$ (PK)")]
        public float PrecioPorKm { get; set; }

        [Display(Name = "Tipo De Servicio")]
        [Required]
        public int TipoDeServicioId { get; set; }

        public string TipoDeServicioDescripcion { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool EstadoDelCosto { get; set; }

        [Required]
        [Display(Name = "Precio De Recargo (PR)")]
        public float PrecioDeRecargo { get; set; }

        [Required]
        [Display(Name = "Precio de ida y regreso (PIR)")]
        public float PrecioDeRegreso { get; set; }
    }

    public class CostoBancarioIndexViewModel 
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaDeInicio { get; set; }

        [Required]
        [Display(Name = "Fecha de Fin")]
        [DataType(DataType.Date)]
        public DateTime FechaDeFin { get; set; }

        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Monto Desde C$ (MD)")]
        [Range(120, 10000, ErrorMessage = "Por Seguridad solo hacemos gestiones de {1} a {2}. " +
    "Contactate con nosotros si deseas enviar una cantidad mayor")]
        public decimal MontoDesde { get; set; }

        [Required]
        [Display(Name = "Monto Hasta C$ (MH)")]
        [Range(120, 10000, ErrorMessage = "Por Seguridad solo hacemos gestiones de {1} a {2}. " +
    "Contactate con nosotros si deseas enviar una cantidad mayor")]
        public decimal MontoHasta { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Display(Name = "Porcentaje (P)")]
        public float Porcentaje { get; set; }

        [Display(Name = "Valor C$ (V)")]
        public int Valor { get; set; }

        [Required]
        [Display(Name = "Precio De Recargo C$ (PR)")]
        public float PrecioDeRecargo { get; set; }

        [Required]
        [Display(Name = "Precio ida y regreso C$ (PIR)")]
        public float PrecioDeRegreso { get; set; }

        [Required]
        [Display(Name = "Tipo De Servicio")]
        public int TipoDeServicioId { get; set; }

        public string TipoDeServicioDescripcion { get; set; }
    }
}