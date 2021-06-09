using System;
using System.ComponentModel.DataAnnotations;
using  System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models
{
    public class Persona
    {
        [Key]
        public int PersonaId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
      
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(30)]
        public string PrimerNombre { get; set; }

        [StringLength(30)]
        public string SegundoNombre { get; set; }

        [Required]
        [StringLength(30)]
        public string PrimerApellido { get; set; }

        [Required]
        [StringLength(30)]
        public string SegundoApellido { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        public byte Foto { get; set; }

        [Required]
        [StringLength(9)]//Masculino-Femenino
        public string Sexo { get; set; }

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; }

        [Required]
        [StringLength(16)]//132-090994-2000F
        public string Cedula { get; set; }

        public DateTime FechaIngresoDeLaPersona { get; set; }

        //Campos Calculados

        [NotMapped]
        public string NombreCompleto { get { return PrimerNombre + " " + SegundoNombre + " " + PrimerApellido+ " "+ SegundoApellido; } }
    }
}