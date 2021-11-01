using System;
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
        }

        [Key]
        public int Id { get; set; }

        [StringLength(250)]
        public string DescripcionDeEnvio { get; set; }

        public DateTime FechaDelEnvio { get; set; }

        [StringLength(200)]
        public string DireccionDeRecepcion { get; set; }

        [StringLength(300)]
        public string DireccionDeEntrega { get; set; }

        public double DistanciaEntregaRecep { get; set; }


        public string NombresDelReceptor { get; set; }

        public string ApellidosDelReceptor { get; set; }

        public string CedulaDelReceptor { get; set; }

        [Display(Name = "El peso es menor a 50 Libras?")]
        public bool Peso { get; set; }

        [Required]
        [Display(Name = "Monto de dinero de la gestión")]
        public Decimal MontoDeDinero { get; set; }

        [Required]
        [Display(Name = "Monto total del envio")]
        public decimal MontoTotalDelEnvio { get; set; }

        public string TelefonoDelReceptor { get; set; }

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
        public bool Cambio { get; set; }

        public double PrecioDeRecargo { get; set; }

        public short EstadoDelEnvio { get; set; }

        public int MotocicletaId { get; set; }

        public int AsistenteId { get; set; }

        public int ClienteId { get; set; }

        public int MotorizadoId { get; set; }
        public int TipoDePagoId { get; set; }

        public int ServicioId { get; set; }

        [Display(Name = "Lugar Origen")]
        public int LugarOrigenId { get; set; }

        [Display(Name = "Lugar Destino")]
        public int LugarDestinoId { get; set; }

        [Display(Name = "Cotización")]
        public int CotizacionId { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Motocicleta Motocicleta { get; set; }
        public virtual Asistente Asistente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pago> Pagos { get; set; }
        public virtual Motorizado Motorizado { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seguimiento> Seguimientos { get; set; }
        public virtual Servicio Servicio { get; set; }
        public virtual Lugar LugarOrigen { get; set; }
        public virtual Lugar LugarDestino { get; set; }
    }
}