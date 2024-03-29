﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    [Table("Envios")]
    public class Envio
    {
        public Envio()
        {
            Pagos = new HashSet<Pago>();
            Seguimientos = new HashSet<Seguimiento>();
            CodigoDeEnvio = "-";
        }

        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string CodigoDeEnvio { get; set; }

        [StringLength(250)]
        [Required]
        [Display(Name = "Descripción del envío")]
        public string DescripcionDeEnvio { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime FechaDelEnvio { get; set; }

        [Required]
        [Display(Name = "Distancia")]
        public double DistanciaEntregaRecep { get; set; }

        [Required]
        [Display(Name = "Nombres del receptor")]
        public string NombresDelReceptor { get; set; }

        [Display(Name = "Cédula receptor")]
        public string CedulaDelReceptor { get; set; }

        [Display(Name = "El peso es menor a 50 Libras?")]
        public bool Peso { get; set; }

        [Required]
        [Display(Name = "Monto de la gestión")]
        public decimal MontoDeDinero { get; set; }

        [Required]
        [Display(Name = "Monto total del envio")]
        public decimal MontoTotalDelEnvio { get; set; }

        [Required]
        [Display(Name = "Celular del receptor")]
        [MinLength(8)]
        [MaxLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string TelefonoDelReceptor { get; set; }

        [Display(Name = "¿Urgente?")]
        public bool EsUrgente { get; set; }

        [Display(Name = "¿Es ida y regreso?")]
        public bool DebeRegresarATienda { get; set; }

        [Display(Name = "¿El motorizado recibirá algun dinero?")]
        public bool DebeRecibirDinero { get; set; }

        [Display(Name = "¿Cuanto recibirá?")]
        public decimal MontoARecibir { get; set; }
        [Display(Name = "¿Cuanto de cambio?")]
        public decimal MontoCambio { get; set; }

        [Display(Name = "¿El motorizado necesita cambio?")]
        public bool DebeRecibirCambio { get; set; }

        [Display(Name = "Estado")]
        public short EstadoDelEnvio { get; set; }

        public int? MotocicletaId { get; set; }

        public int? AsistenteId { get; set; }

        public int ClienteId { get; set; }

        [Display(Name = "Motorizado")]
        public int? MotorizadoId { get; set; }

        [Required]
        [Display(Name = "Tipo de pago")]
        public int TipoDePagoId { get; set; }

        [Required]
        [Display(Name = "Tipo de servicio")]
        public int TipoDeServicioId { get; set; }

        [Display(Name = "Servicio")]
        public int? ServicioId { get; set; }

        [Display(Name = "Lugar Origen")]
        public int LugarOrigenId { get; set; }

        [Display(Name = "Lugar Destino")]
        public int LugarDestinoId { get; set; }

        [Display(Name = "Cotización")]
        public int? CotizacionId { get; set; }

        [Display(Name ="Al crédito")]
        public bool EsAlCredito { get; set; }

        [StringLength(150)]
        public string MotivoDeRechazo { get; set; }

        public int? CreditoId { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Credito Credito { get; set; }
        public virtual Motocicleta Motocicleta { get; set; }
        public virtual Asistente Asistente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pago> Pagos { get; set; }
        public virtual Motorizado Motorizado { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seguimiento> Seguimientos { get; set; }
        public virtual Servicio Servicio { get; set; }
        public virtual TipoDeServicio TipoDeServicio { get; set; }

        public virtual TipoDePago TipoDePago { get; set; }
        public virtual Lugar LugarOrigen { get; set; }
        public virtual Lugar LugarDestino { get; set; }
    }
}