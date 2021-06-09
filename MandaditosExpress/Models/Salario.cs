using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class Salario
    {
        [Key]
        public int Id { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaDeHasta { get; set; }
        public int MotorizadoId { get; set; }
        public bool Estado { get; set; }
        public int PorcentajePorEnvio { get; set; }

        public virtual Motorizado Motorizado { get; set; }
    }
}