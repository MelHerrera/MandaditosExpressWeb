﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("Monedas")]
    public class Moneda
    {
        public Moneda()
        {
            this.Pagos = new HashSet<Pago>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre De La Moneda")]
        public string NombreDeMoneda { get; set; }

        [Required]
        public string Abreviatura { get; set; }


        public bool Estado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pago> Pagos { get; set; }
    }
}