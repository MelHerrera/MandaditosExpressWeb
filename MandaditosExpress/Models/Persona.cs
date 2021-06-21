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
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(30)]
        public string PrimerNombre { get; set; }

        [StringLength(30)]
        public string SegundoNombre { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(30)]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(30)]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        public byte[] Foto { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(9)]//Masculino-Femenino
        public string Sexo { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(200)]
        public string Direccion { get; set; }

        [StringLength(16)]//132-090994-2000F
        public string Cedula { get; set; }

        public DateTime FechaIngreso { get; set; }

        //Campos Calculados

        [NotMapped]
        public string NombreCompleto { get { return PrimerNombre + " " + SegundoNombre + " " + PrimerApellido+ " "+ SegundoApellido; } }
    }
}