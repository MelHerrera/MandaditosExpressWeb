using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace MandaditosExpress.Models
{
    [Table("Motocicletas")]
    public class Motocicleta
    {
        public Motocicleta()
        {
            this.Envios = new HashSet<Envio>();
        }

        [Key]
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Color { get; set; }
        public string Modelo { get; set; }
        public short Anio { get; set; }
        public bool EsPropia { get; set; }
        public short Kilometraje { get; set; }
        public DateTime FechaDeIngreso { get; set; }
        public bool EsTemporal { get; set; }
        public DateTime FechaDeValidez { get; set; }
        public int MotorizadoId { get; set; }
        public bool EstadoDeMotocicleta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Envio> Envios { get; set; }
        public virtual Motorizado Motorizado { get; set; }
    }
}