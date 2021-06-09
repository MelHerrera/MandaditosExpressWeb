using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class TipoDeServicio
    {
        public TipoDeServicio()
        {
            this.Servicios = new HashSet<Servicio>();
        }
        [Key]
        public int Id { get; set; }
        public string DescripcionTipoDeServicio { get; set; }
        public bool EstadoTipoDeServicio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Servicio> Servicios { get; set; }
    }
}