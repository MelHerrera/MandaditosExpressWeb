﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models
{
    [Table("Servicios")]
    public class Servicio
    {
        public Servicio()
        {
            Envios = new HashSet<Envio>();
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
        [Display(Name = "Tipo De Servicio")]
        public int TipoDeServicioId { get; set; }

        public virtual ICollection<Envio> Envios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual TipoDeServicio TipoDeServicio { get; set; }
    }
}