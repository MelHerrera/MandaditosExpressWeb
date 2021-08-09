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
            this.Gestiones = new HashSet<Servicio>();
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
        [Display(Name = "Costo por Gasolina")]
        public float CostoDeGasolina { get; set; }

        [Required]
        [Display(Name = "Costo por Asistencia")]
        public float CostoDeAsistencia { get; set; }

        [Required]
        [Display(Name = "Costo por Motorizado")]
        public float CostoDeMotorizado { get; set; }

        [Required]
        [Display(Name = "Distancia Base (Km)")]
        public float DistanciaBase { get; set; }

        [Required]
        [Display(Name = "Precio por Kilometro (C$)")]
        public float PrecioPorKm { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool EstadoDelCosto { get; set; }

        [Required]
        [Display(Name = "Precio Base por Gestion Bancaria")]
        public decimal PrecioBaseGestionBancaria { get; set; }

        [Required]
        [Display(Name = "% Base por Gestion Bancaria")]
        public int PorcentajeBaseGestionBancaria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Servicio> Gestiones { get; set; }
    }
}