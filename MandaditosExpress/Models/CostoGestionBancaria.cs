using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models
{
    [Table("CostosGestionBancaria")]
    public class CostoGestionBancaria
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
        [Display(Name ="Monto Desde C$ (MD)")]
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

        [Display(Name ="Valor C$ (V)")]
        public int Valor { get; set; }

        [Required]
        [Display(Name = "Precio De Recargo C$ (PR)")]
        public float PrecioDeRecargo { get; set; }

        [Required]
        [Display(Name = "Precio ida y regreso C$ (PIR)")]
        public float PrecioDeRegreso{ get; set; }

        [Required]
        [Display(Name = "Tipo De Servicio")]
        public int TipoDeServicioId { get; set; }

        public virtual TipoDeServicio TipoDeServicio { get; set; }
    }
}