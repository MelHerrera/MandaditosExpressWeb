using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models
{
    [Table("VelocidadDeConexiones")]
    public class VelocidadDeConexion
    {
        public VelocidadDeConexion()
        {
            this.Motorizados = new HashSet<Motorizado>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Descripcion { get; set; }

        public bool Estado { get; set; }

        public virtual ICollection<Motorizado> Motorizados { get; set; }
    }
}