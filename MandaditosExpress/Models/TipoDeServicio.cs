using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("TiposDeServicio")]
    public class TipoDeServicio
    {
        public TipoDeServicio()
        {
            this.Servicios = new HashSet<Servicio>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        [Display(Name ="Descripción")]
        public string DescripcionTipoDeServicio { get; set; }

        [Display(Name ="Estado")]
        public bool EstadoTipoDeServicio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Servicio> Servicios { get; set; }
    }
}