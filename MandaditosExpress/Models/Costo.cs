﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("Costos")]
    public class Costo
    {
        public Costo()
        {
            this.FechaDeInicio = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaDeInicio { get; set; }

        [Required]
        [Display(Name = "Fecha de Fin")]
        [DataType(DataType.Date)]
        public DateTime FechaDeFin { get; set; }

        [Required]
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string  Descripcion { get; set; }

        [Display(Name = "Costo por Gasolina")]
        public float CostoDeGasolina { get; set; }

        [Display(Name = "Costo por Asistencia")]
        public float CostoDeAsistencia { get; set; }

        [Display(Name = "Costo por Motorizado")]
        public float CostoDeMotorizado { get; set; }

        [Required]
        [Display(Name = "Distancia Base (Km)")]
        public float DistanciaBase { get; set; }

        [Required]
        [Display(Name = "Precio por Kilometro (C$)")]
        public float PrecioPorKm { get; set; }

        [Display(Name = "Tipo De Servicio")]
        [Required]
        public int TipoDeServicioId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool EstadoDelCosto { get; set; }

        [Display(Name = "Precio Base por Gestion Bancaria")]
        public decimal PrecioBaseGestionBancaria { get; set; }

        [Display(Name = "% Base por Gestion Bancaria")]
        public int PorcentajeBaseGestionBancaria { get; set; }

        public virtual TipoDeServicio TipoDeServicio { get; set; }
    }
}