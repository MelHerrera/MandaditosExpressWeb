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
        public string Descripcion { get; set; }

        public bool EstadoDeLaDisponibilidad { get; set; }

        public virtual  ICollection<Motorizado> Motorizados { get; set; }
    }
}