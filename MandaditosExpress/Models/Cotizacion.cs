using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("Cotizaciones")]

    public class Cotizacion
    {
        public Cotizacion()
        {
            FechaDeLaCotizacion = DateTime.Now;
            FechaDeValidez = DateTime.Now.AddDays(15);
        }

        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        public string DescripcionDeCotizacion { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaDeLaCotizacion { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaDeValidez { get; set; }

        [StringLength(200)]
        public string DireccionDeOrigen { get; set; }

        [StringLength(200)]
        public string DireccionDestino { get; set; }

        public float DistanciaOrigenDestino { get; set; }

        public bool EsEspecial { get; set; }

        public double MontoTotal { get; set; }

        public int ClienteId { get; set; }

        public int TipoDeServicioId { get; set; }

        public decimal MontoDeDinero { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual TipoDeServicio TipoDeServicio { get; set; }
    }
}