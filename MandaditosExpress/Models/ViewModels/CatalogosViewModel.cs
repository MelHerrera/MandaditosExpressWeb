﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
            var gestion = db.TiposDeServicio.FirstOrDefault(x => x.DescripcionTipoDeServicio.ToUpper().Contains("Banc"));
            GestionBancariaId = gestion!=null ? gestion.Id : -1;
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

        public Lugar LugarDeOrigen { get; set; }
        public Lugar LugarDestino { get; set; }

        public float DistanciaOrigenDestino { get; set; }

        [Required]
        public bool EsEspecial { get; set; }

        [Required]
        public decimal MontoTotal { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Tipo de Servicio")]
        public int TipoDeServicioId { get; set; }

        public decimal MontoDeDinero { get; set; }

        public int GestionBancariaId { get; set; }

        public List<TipoDeServicio> TiposDeServicios { get; set; }
    }
}