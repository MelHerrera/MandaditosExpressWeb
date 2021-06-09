using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class Cotizacion
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        public string DescripcionDeCotizacion { get; set; }

        public DateTime FechaDeLaCotizacion { get; set; }

        [StringLength(200)]
        public string DireccionDeEntrega { get; set; }

        [StringLength(200)]
        public string DireccionDeRecepcion { get; set; }

        public double DistanciaEntregaRecep { get; set; }

        public bool EsUrgente { get; set; }

        public decimal PrecioDeRecargo { get; set; }

        public int GestionId { get; set; }

        public double MontoTotal { get; set; }

        public int ClienteId { get; set; }

        public int ServicioId { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Servicio Servicio { get; set; }
    }
}