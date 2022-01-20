using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MandaditosExpress.Models.ViewModels
{
    public class EnviosCreditoViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Descripción")]
        public string DescripcionDeEnvio { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime FechaDelEnvio { get; set; }

        [Required]
        [Display(Name = "Monto")]
        public decimal MontoTotalDelEnvio { get; set; }
    }

    public class EnviosCreditoConsultaViewModel
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int ClienteId { get; set; }
    }

    public class EnviosMensualesViewModel {
        public int Dia { get; set; }

        [Display(Name = "Total/Fecha")]
        public int Total { get; set; }

        [Display(Name ="Solicitud")]
        public int EnviosSolicitud { get; set; }

        [Display(Name = "En Proceso")]
        public int EnviosProceso { get; set; }

        [Display(Name = "Realizados")]
        public int EnviosFinalizado { get; set; }

        [Display(Name = "Rechazados")]
        public int EnviosRechazado { get; set; }
    }
}