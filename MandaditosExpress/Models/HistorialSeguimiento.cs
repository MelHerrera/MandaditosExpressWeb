using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("HistorialesDeSeguimiento")]
    public class HistorialSeguimiento
    {
        [Key]
        public int Id { get; set; }
        public string LongitudDeOrigen { get; set; }
        public string LatitudDeOrigen { get; set; }
        public string LongitudDeDestino { get; set; }
        public string LatitudDeDestino { get; set; }
        public TimeSpan HoraDeInicio { get; set; }
        public TimeSpan HoraDeFin { get; set; }

        public int EnvioId { get; set; }

    }
}