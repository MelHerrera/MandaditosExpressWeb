using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("TiposDePago")]
    public class TipoDePago
    {
        public TipoDePago()
        {
            this.Pagos = new HashSet<Pago>();
        }
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool EstadoTipoDePago { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pago> Pagos { get; set; }
    }
}