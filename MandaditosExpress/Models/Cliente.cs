using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models
{
    [Table("Clientes")]
    public class Cliente:Persona
    {
        public Cliente()
        {
            this.Envios = new HashSet<Envio>();
            this.Cotizaciones = new HashSet<Cotizacion>();
            this.Creditos = new HashSet<Credito>();
        }

        public bool EsEmpresa { get; set; }

        public string NombreDeLaEmpresa { get; set; }

        [StringLength(14)]//J0130000006891
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