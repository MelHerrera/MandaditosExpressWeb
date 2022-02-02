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

        [Required]
        [Display(Name = "¿Es Empresa?")]
        public bool EsEmpresa { get; set; }

        [Display(Name = "Nombre de Empresa")]
        public string NombreDeLaEmpresa { get; set; }

        [StringLength(16,ErrorMessage ="Excedio la Longitud Permitida")]//J0130000006891--se extendio a 16, ya que, de algunas personas su ruc es la cedula
        [Display(Name = "Número RUC")]
        public string RUC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Envio> Envios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cotizacion> Cotizaciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Credito> Creditos { get; set; }
    }
}