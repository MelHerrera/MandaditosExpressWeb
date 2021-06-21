using System.Collections.Generic;
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

        public bool EsAfiliado { get; set; }
        public short EstadoDeAfiliado { get; set; }
        public DateTime FechaDeAfiliacion { get; set; }
        public DateTime FechaIngresoDelMotorizado { get; set; }
        public bool EstadoDeMotorizado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Motocicleta> Motocicletas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Salario> Salarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Envio> Envios { get; set; }
    }
}