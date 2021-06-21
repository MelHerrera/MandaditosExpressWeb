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
            this.Pagos = new HashSet<Pago>();
            this.Seguimientos = new HashSet<Seguimiento>();
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

        public double Ancho { get; set; }

        public double Alto { get; set; }

        public double Peso { get; set; }

        public Decimal MontoDeDinero { get; set; }

        public string TelefonoDelReceptor { get; set; }

        public DateTime FechaDeEntrega { get; set; }

        public bool EsUrgente { get; set; }

        public TimeSpan HoraDeEntrega { get; set; }

        public double PrecioDeRecargo { get; set; }

        public short EstadoDelEnvio { get; set; }

        public int MotocicletaId { get; set; }

        public int AsistenteId { get; set; }

        public int ClienteId { get; set; }

        public int MotorizadoId { get; set; }

        public string Credito { get; set; }

        public int ServicioId { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Motocicleta Motocicleta { get; set; }
        public virtual Asistente Asistente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pago> Pagos { get; set; }
        public virtual Motorizado Motorizado { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seguimiento> Seguimientos { get; set; }
        public virtual Servicio Servicio { get; set; }
    }
}