using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MandaditosExpress.Models
{
    [Table("Seguimientos")]
    public class Seguimiento
    {
        public Seguimiento()
        {
            this.DetallesDeSeguimiento = new HashSet<DetalleDeSeguimiento>();
        }

        [Key]
        public int Id { get; set; }
        public string LongitudDeOrigen { get; set; }
        public string LatitudDeOrigen { get; set; }
        public string LongitudDeDestino { get; set; }
        public string LatitudDeDestino { get; set; }
        public TimeSpan HoraDeInicio { get; set; }
        public TimeSpan HoraDeFin { get; set; }
        public int EnvioId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleDeSeguimiento> DetallesDeSeguimiento { get; set; }
        public virtual Envio Envio { get; set; }
    }
}