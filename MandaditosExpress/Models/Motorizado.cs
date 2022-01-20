using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System;
using MandaditosExpress.Models.Enum;

namespace MandaditosExpress.Models
{
    [Table("Motorizados")]
    public class Motorizado:Persona
    {
        public Motorizado()
        {
            Motocicletas = new HashSet<Motocicleta>();
            Salarios = new HashSet<Salario>();
            Envios = new HashSet<Envio>();
            FechaDeAfiliacion = DateTime.Parse("01/01/1900 00:00:00");
            FechaRechazoAfiliacion = DateTime.Parse("01/01/1900 00:00:00");
            EstadoDelMotorizado = (short) EstadoDeMotorizadoEnum.Inactivo;//el estado se refiere a inactivo, activo o ocupado porque esta realizado una entrega
            EstadoDeAfiliado = (short)EstadoDeAfiliadoEnum.NoAplica;
        }

        [Required]
        [Display(Name ="¿Es Afiliado?")]
        public bool EsAfiliado { get; set; }

        [Display(Name = "Estado de Afiliación")]
        public short EstadoDeAfiliado { get; set; }

        [Display(Name = "Fecha de Afiliación")]
        [DataType(DataType.DateTime)]
        public DateTime FechaDeAfiliacion { get; set; }

        [Display(Name = "Fecha de Rechazo")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRechazoAfiliacion { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Estado")]
        public short EstadoDelMotorizado { get; set; }

        [Display(Name = "Velocidad de Internet")]
        public int? VelocidadDeConexionId { get; set; }

        [Display(Name = "Disponibilidad")]
        public int? DisponibilidadId { get; set; }

        public virtual CalidadDeConexion VelocidadDeConexion { get; set; }

        public virtual Disponibilidad Disponibilidad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Motocicleta> Motocicletas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Salario> Salarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Envio> Envios { get; set; }
    }
}