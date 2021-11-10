using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("Pagos")]
    public class Pago
    {
        public Pago()
        {
            FechaDePago = DateTime.Now;
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
        [Display(Name = "Tipo de Pago")]
        public int TipoDePagoId { get; set; }

        [Display(Name = "Envio")]
        public int? EnvioId { get; set; }

        [Display(Name = "Créditp")]
        public int? CreditoId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool EstadoDelPago { get; set; }

        public virtual Moneda Moneda { get; set; }
        public virtual TipoDePago TipoDePago { get; set; }
        public virtual Envio Envio { get; set; }
        public virtual Credito Credito { get; set; }
    }
}