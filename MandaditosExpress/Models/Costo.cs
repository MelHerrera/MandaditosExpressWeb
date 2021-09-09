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

        [Display(Name = "Costo por Gasolina")]
        [Column(TypeName = "decimal(4, 2)")]
        public decimal CostoDeGasolina { get; set; }

        [Display(Name = "Costo por Asistencia")]
        [Column(TypeName = "decimal(4, 2)")]
        public decimal CostoDeAsistencia { get; set; }

        [Display(Name = "Costo por Motorizado")]
        [Column(TypeName = "decimal(4, 2)")]
        public decimal CostoDeMotorizado { get; set; }

        [Required]
        [Display(Name = "Distancia Base (Km)")]
        public float DistanciaBase { get; set; }

        [Required]
        [Display(Name = "Precio por Km (C$)")]
        public float PrecioPorKm { get; set; }

        [Display(Name = "Tipo De Servicio")]
        [Required]
        public int TipoDeServicioId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool EstadoDelCosto { get; set; }

        [Required]
        [Display(Name = "Precio De Recargo")]
        public float PrecioDeRecargo { get; set; }

        public virtual  TipoDeServicio TipoDeServicio { get; set; }
    }
}