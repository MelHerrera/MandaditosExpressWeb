using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class HistorialSeguimiento
    {
        public HistorialSeguimiento()
        {
            this.HistorialDetalleDeSeguimiento = new HashSet<HistorialDetalleSeguimiento>();
        }

        [Key]
        public int Id { get; set; }
        public string LongitudDeOrigen { get; set; }
        public string LatitudDeOrigen { get; set; }
        public string LongitudDeDestino { get; set; }
        public string LatitudDeDestino { get; set; }
        public TimeSpan HoraDeInicio { get; set; }
        public TimeSpan HoraDeFin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistorialDetalleSeguimiento> HistorialDetalleDeSeguimiento { get; set; }
    }
}