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
            this.Costos = new HashSet<Costo>();
            this.Cotizaciones = new HashSet<Cotizacion>();
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

        public virtual ICollection<Costo> Costos { get; set; }

        public virtual ICollection<Cotizacion> Cotizaciones { get; set; }
    }
}