using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models
{
    [Table("DetallesDeSeguimiento")]

    public class DetalleDeSeguimiento
    {
        [Key]
        public int Id { get; set; }
        public int SeguimientoId { get; set; }
        public DateTime HoraDeInicio { get; set; }
        public DateTime HoraDeEntrega { get; set; }
        public string LongitudActual { get; set; }
        public string LatitudActual { get; set; }

        public virtual Seguimiento Seguimiento { get; set; }
    }
}