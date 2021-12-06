﻿using System;
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
}