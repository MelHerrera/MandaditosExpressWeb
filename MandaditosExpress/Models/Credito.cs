using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class Credito
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaDeInicio { get; set; }

        public DateTime FechaDeVencimiento { get; set; }

        public bool EstadoDelCredito { get; set; }

        public DateTime FechaDeCancelacion { get; set; }

        public int ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}