﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System;

namespace MandaditosExpress.Models
{
    [Table("Motorizados")]
    public class Motorizado:Persona
    {
        public Motorizado()
        {
            this.Motocicletas = new HashSet<Motocicleta>();
            this.Salarios = new HashSet<Salario>();
            this.Envios = new HashSet<Envio>();
        }

        [Required]
        [Display(Name ="¿Es Afiliado?")]
        public bool EsAfiliado { get; set; }

        [Display(Name = "Estado de Afiliación")]
        public short EstadoDeAfiliado { get; set; }

        [Display(Name = "Fecha de Afiliación")]
        [DataType(DataType.DateTime)]
        public DateTime FechaDeAfiliacion { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Estado")]
        public bool EstadoDelMotorizado { get; set; }

        [Display(Name = "Velocidad de Internet")]
        public int VelocidadDeConexionId { get; set; }

        [Display(Name = "Disponibilidad")]
        public int DisponibilidadId { get; set; }

        public virtual VelocidadDeConexion VelocidadDeConexion { get; set; }

        public virtual Disponibilidad Disponibilidad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Motocicleta> Motocicletas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Salario> Salarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Envio> Envios { get; set; }
    }
}