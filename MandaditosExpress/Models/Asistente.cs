using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class Asistente:Persona
    {
        public Asistente()
        {
            this.Envios = new HashSet<Envio>();
        }

        [Key]
        public int AsistenteId { get; set; }


        public DateTime FechaIngresoDeAsistente { get; set; }
        public bool EstadoDeAsistente { get; set; }
        public DateTime FechaDeBaja { get; set; }

        public virtual ICollection<Envio> Envios { get; set; }
    }
}