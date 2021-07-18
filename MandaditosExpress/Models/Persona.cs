using System;
using System.ComponentModel.DataAnnotations;
using  System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models
{
    [Table("Personas")]
    public class Persona
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Campo Obligatorio")]
        [DataType(DataType.EmailAddress)]
        [Display(Name="Correo Electronico")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(30)]
        [Display(Name = "Primer Nombre")]
        public string PrimerNombre { get; set; }

        [StringLength(30)]
        [Display(Name = "Segundo Nombre")]
        public string SegundoNombre { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(30)]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(30)]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        public byte[] Foto { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(9,ErrorMessage ="Excedio la Longitud Permitida")]//Masculino-Femenino
        public string Sexo { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [StringLength(16)]//132-090994-2000F
        public string Cedula { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        //Campos Calculados

        [NotMapped]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get { return PrimerNombre + " " + SegundoNombre + " " + PrimerApellido+ " "+ SegundoApellido; } }
    }
}