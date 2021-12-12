using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("Creditos")]

    public class Credito
    {
        public Credito()
        {
            this.FechaDeCancelacion = DateTime.Parse("01/01/1900");
            Pagos = new HashSet<Pago>();
            Envios = new HashSet<Envio>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha De Inicio")]
        public DateTime FechaDeInicio { get; set; }

        [Display(Name = "Fecha De Vencimiento")]
        public DateTime FechaDeVencimiento { get; set; }

        [Display(Name = "Estado")]
        public bool EstadoDelCredito { get; set; }

        [Display(Name = "Fecha De Cancelacion")]
        public DateTime FechaDeCancelacion { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }

        public virtual ICollection<Envio> Envios { get; set; }
    }
}