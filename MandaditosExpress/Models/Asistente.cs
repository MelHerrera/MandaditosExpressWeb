using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models
{
    [Table("Asistentes")]
    public class Asistente:Persona
    {

        public Asistente()
        {
            this.Envios = new HashSet<Envio>();
        }

        public bool EstadoDeAsistente { get; set; }
        public DateTime FechaDeBaja { get; set; }

        public virtual ICollection<Envio> Envios { get; set; }
    }
}