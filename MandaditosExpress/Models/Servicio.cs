using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models
{
    [Table("Servicios")]
    public class Servicio
    {
        public Servicio()
        {
            this.Envios = new HashSet<Envio>();
            this.Cotizaciones = new HashSet<Cotizacion>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        [Display(Name ="Descripción")]
        public string DescripcionDelServicio { get; set; }

        public bool Estado { get; set; }

        [Required]
        public int TipoDeServicioId { get; set; }


        public virtual ICollection<Envio> Envios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cotizacion> Cotizaciones { get; set; }

        public virtual TipoDeServicio TipoDeServicio { get; set; }
    }
}