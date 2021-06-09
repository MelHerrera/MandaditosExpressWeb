using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class Costo
    {
        public Costo()
        {
            this.Gestiones = new HashSet<Servicio>();
        }

        [Key]
        public int Id { get; set; }

        public DateTime FechaDeInicio { get; set; }

        public DateTime FechaDeFin { get; set; }

        public decimal CostoDeGasolina { get; set; }

        public decimal CostoDeAsistencia { get; set; }

        public decimal CostoDeMotorizado { get; set; }

        public double DistanciaBase { get; set; }

        public decimal PrecioPorKm { get; set; }

        public bool EstadoDelCosto { get; set; }

        public decimal PrecioBaseGestionBancaria { get; set; }

        public int PorcentajeBaseGestionBancaria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Servicio> Gestiones { get; set; }
    }
}