using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models
{
    [Table("Disponibilidades")]
    public class Disponibilidad
    {
        public Disponibilidad()
        {
            this.Motorizados = new HashSet<Motorizado>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Estado")]
        public bool EstadoDeLaDisponibilidad { get; set; }
        
        public virtual  ICollection<Motorizado> Motorizados { get; set; }
    }
}