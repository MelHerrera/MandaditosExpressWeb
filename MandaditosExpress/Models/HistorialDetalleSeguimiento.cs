using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class HistorialDetalleSeguimiento
    {
        [Key]
        public int Id { get; set; }
        public int HistorialSeguimientoId { get; set; }
        public TimeSpan HoraDeInicio { get; set; }
        public TimeSpan HoraDeEntrega { get; set; }
        public string LongitudActual { get; set; }
        public string LatitudActual { get; set; }

        public virtual HistorialSeguimiento HistorialSeguimientos { get; set; }
    }
}