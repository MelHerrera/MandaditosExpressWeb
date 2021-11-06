using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MandaditosExpress.Models
{
    [Table("Lugares")]
    public class Lugar
    {
        public Lugar()
        {
            Cotizaciones = new HashSet<Cotizacion>();
            Envios = new HashSet<Envio>();
        }
        [Key]
        public int Id { get; set; }

        [Display(Name ="Descripción del Lugar")]
        public string Descripcion { get; set; }

        public string Direccion { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public virtual ICollection<Cotizacion> Cotizaciones { get; set; }
        public virtual ICollection<Envio> Envios { get; set; }
    }
}