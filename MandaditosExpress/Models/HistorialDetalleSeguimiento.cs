using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("HistorialDetallesDeSeguimiento")]
    public class HistorialDetalleSeguimiento
    {
        [Key]
        public int Id { get; set; }
        public int HistorialSeguimientoId { get; set; }

        public TimeSpan HoraActual { get; set; }

        public string LongitudActual { get; set; }
        public string LatitudActual { get; set; }

        public virtual HistorialSeguimiento HistorialSeguimientos { get; set; }
    }
}