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
        [Required]
        public string Placa { get; set; }

        [Required]
        [MaxLength(7)]
        public string Color { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El campo Año es obligatorio.")]
        public int Anio { get; set; }

        [Required]
        public bool EsPropia { get; set; }

        [Required]
        public int Kilometraje { get; set; }
        public DateTime FechaDeIngreso { get; set; }

        [Required]
        public bool EsTemporal { get; set; }
        public DateTime FechaDeValidez { get; set; }
        public int MotorizadoId { get; set; }
        public bool EstadoDeMotocicleta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Envio> Envios { get; set; }
        public virtual Motorizado Motorizado { get; set; }
    }
}