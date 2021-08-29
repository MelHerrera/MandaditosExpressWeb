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
        public CostoGestionBancaria()
        {
            TiposDeServicio = new HashSet<TipoDeServicio>();
        }
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

        [Required]
        [Display(Name ="Monto Desde")]
        public decimal MontoDesde { get; set; }

        [Required]
        [Display(Name = "Monto Hasta")]
        public decimal MontoHasta { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Required]
        public float Porcentaje { get; set; }

        [Required]
        [Display(Name = "Precio De Recargo")]
        public float PrecioDeRecargo { get; set; }

        [Required]
        public int TipoDeServicioId { get; set; }

        public virtual ICollection<TipoDeServicio> TiposDeServicio { get; set; }
    }
}