using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("Costos")]
    public class Costo
    {
        public Costo()
        {
            this.FechaDeInicio = DateTime.Now;
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
        public string  Descripcion { get; set; }

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

        [Required]
        [Display(Name = "Estado")]
        public bool EstadoDelCosto { get; set; }

        [Required]
        [Display(Name = "Precio De Recargo (PR)")]
        public float PrecioDeRecargo { get; set; }

        [Required]
        [Display(Name = "Precio de ida y regreso (PIR)")]
        public float PrecioDeRegreso { get; set; }

        public virtual  TipoDeServicio TipoDeServicio { get; set; }
    }
}