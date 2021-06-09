using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class Cliente:Persona
    {
        public Cliente()
        {
            this.Envios = new HashSet<Envio>();
            this.Cotizaciones = new HashSet<Cotizacion>();
            this.Creditos = new HashSet<Credito>();
        }

        [Key]
        public int ClienteId { get; set; }

        public bool EsEmpresa { get; set; }

        public string NombreDeLaEmpresa { get; set; }

        public string RUC { get; set; }

        public DateTime FechaIngresoDelCliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Envio> Envios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cotizacion> Cotizaciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Credito> Creditos { get; set; }
    }
}