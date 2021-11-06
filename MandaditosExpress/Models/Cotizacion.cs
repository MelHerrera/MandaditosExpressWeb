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
        private MandaditosDB db;
        public Cotizacion()
        {
            db = new MandaditosDB();
            FechaDeLaCotizacion = DateTime.Now;
            FechaDeValidez = DateTime.Now.AddDays(15);
            //TiposDeServicios = db.TiposDeServicio.ToList();
            var gestion = db.TiposDeServicio.FirstOrDefault(x => x.DescripcionTipoDeServicio.ToUpper().Contains("Banc"));
            GestionBancariaId = gestion != null ? gestion.Id : -1;
        }

        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Descripción")]
        public string DescripcionDeCotizacion { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaDeLaCotizacion { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime FechaDeValidez { get; set; }
        public float DistanciaOrigenDestino { get; set; }

        [Required]
        public bool EsEspecial { get; set; }

        [Required]
        public decimal MontoTotal { get; set; }

        public int? LugarOrigenId { get; set; }
        public int? LugarDestinoId { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Tipo de Servicio")]
        public int TipoDeServicioId { get; set; }

        public decimal MontoDeDinero { get; set; }

        public int GestionBancariaId { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Lugar LugarOrigen { get; set; }
        public virtual Lugar LugarDestino { get; set; }

        public virtual TipoDeServicio TipoDeServicio { get; set; }

    }
}