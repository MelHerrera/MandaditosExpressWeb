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
        public int TipoDeServicioId { get; set; }

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
            FechaDePago = DateTime.Now;
            Envios = new HashSet<EnvioPagoViewModel>();
            Creditos = new HashSet<CreditoViewModel>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Número De Pago")]
        [MaxLength(6)]
        public string NumeroDePago { get; set; }

        [Required]
        [Display(Name = "Fecha De Pago")]
        public DateTime FechaDePago { get; set; }

        [Required]
        [Display(Name = "Monto a pagar")]
        public double MontoDelPago { get; set; }

        [Display(Name = "Cambio de la Moneda")]
        public double Cambio { get; set; }

        [Required]
        [Display(Name = "Moneda")]
        public int MonedaId { get; set; }

        [Required]
        [Display(Name = "Método de Pago")]
        public int TipoDePagoId { get; set; }

        [Display(Name = "Envíos del cliente")]
        public int? EnvioId { get; set; }

        [Display(Name = "Créditos del cliente")]
        public int? CreditoId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool EstadoDelPago { get; set; }

        public virtual  ICollection<MonedaViewModel> Monedas { get; set; }
        public virtual ICollection<TipoDePagoViewModel> TiposDePago { get; set; }
        public virtual ICollection<EnvioPagoViewModel> Envios { get; set; }
        public virtual ICollection<CreditoViewModel> Creditos { get; set; }

        public virtual ICollection<ClientePagoViewModel> Clientes { get; set; }
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
        [Display(Name ="Código")]
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

        public string NombreCompletoCliente { get; set; }
    }
}