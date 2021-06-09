using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class Servicio
    {
        public Servicio()
        {
            this.Envios = new HashSet<Envio>();
            this.Cotizaciones = new HashSet<Cotizacion>();
        }

        [Key]
        public int Id { get; set; }
        public string DescripcionDelServicio { get; set; }
        public int TipoDeServicioId { get; set; }
        public double MontoTotalDelServicio { get; set; }
        public int CostoId { get; set; }

        public virtual Costo Costo { get; set; }
        public virtual TipoDeServicio TipoDeServicio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Envio> Envios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cotizacion> Cotizaciones { get; set; }
    }
}