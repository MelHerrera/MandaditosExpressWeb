using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models.ViewModels
{
    public class CotizacionViewModel
    {
        private MandaditosDB db;
        public CotizacionViewModel()
        {
            db = new MandaditosDB();
            FechaDeLaCotizacion = DateTime.Now;
            FechaDeValidez = DateTime.Now.AddDays(15);
            TiposDeServicios = db.TiposDeServicio.ToList();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        public string DescripcionDeCotizacion { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaDeLaCotizacion { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaDeValidez { get; set; }

        [StringLength(200)]
        public string DireccionDeOrigen { get; set; }

        [StringLength(200)]
        public string DireccionDestino { get; set; }

        public float DistanciaOrigenDestino { get; set; }

        [Required]
        public bool EsEspecial { get; set; }

        [Required]
        public double MontoTotal { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Tipo de Servicio")]
        public int TipoDeServicioId { get; set; }

        public decimal MontoDeDinero { get; set; }
        public List<TipoDeServicio> TiposDeServicios { get; set; }
    }

    public class TiposDeServicioViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        [Display(Name = "Descripción")]
        public string DescripcionTipoDeServicio { get; set; }

        [Display(Name = "Estado")]
        public bool EstadoTipoDeServicio { get; set; }
    }
}