using MandaditosExpress.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandaditosExpress.Models.ViewModels
{
    public class ClienteViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electronico")]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Primer Nombre")]
        public string PrimerNombre { get; set; }

        [StringLength(30)]
        [Display(Name = "Segundo Nombre")]
        public string SegundoNombre { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        public byte[] Foto { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "Excedio la Longitud Permitida")]//Masculino-Femenino
        public string Sexo { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [StringLength(16)]//132-090994-2000F
        public string Cedula { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "¿Es Empresa?")]
        public bool EsEmpresa { get; set; }

        [Display(Name = "Nombre de Empresa")]
        public string NombreDeLaEmpresa { get; set; }

        [StringLength(14, ErrorMessage = "Excedio la Longitud Permitida")]//J0130000006891
        [Display(Name = "Número RUC")]
        public string RUC { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(typeof(bool), "true","True",ErrorMessage ="Debe Aceptar los Terminos y Condiciones")]
        public bool AceptaTerminos { get; set; }
    }

    public class ClientePagoViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electronico")]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [StringLength(16)]//132-090994-2000F
        public string Cedula { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [NotMapped]
        public string Nombres { get; set; }

    }


    public class MotorizadoViewModel
    {
        public MotorizadoViewModel()
        {
            Color = "#3399FF";
            FechaDeAfiliacion = DateTime.Parse("01/01/1900 00:00:00");//default value
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electronico")]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Primer Nombre")]
        public string PrimerNombre { get; set; }

        [StringLength(30)]
        [Display(Name = "Segundo Nombre")]
        public string SegundoNombre { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        public byte[] Foto { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "Excedio la Longitud Permitida")]//Masculino-Femenino
        public string Sexo { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [StringLength(16)]//132-090994-2000F
        public string Cedula { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [Display(Name = "¿Es Afiliado?")]
        public bool EsAfiliado { get; set; }

        [Required]
        [Display(Name = "Estado de Afiliación")]
        public short EstadoDeAfiliado { get; set; }

        [Required]
        [Display(Name ="Calidad de su Internet")]
        public int VelocidadDeConexionId { get; set; }

        [Required]
        [Display(Name ="Disponibilidad")]
        public int DisponibilidadId { get; set; }

        [Display(Name = "Fecha de Afiliación")]
        [DataType(DataType.DateTime)]
        public DateTime FechaDeAfiliacion { get; set; }

        [Display(Name ="¿Es Propia?")]
        public bool EsPropia { get; set; }

        [Required]
        public string Placa { get; set; }

        [Required]
        [MaxLength(7)]
        public string Color { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El campo Año es obligatorio.")]
        public int Anio { get; set; }

        [Required]
        public int Kilometraje { get; set; }
        public DateTime FechaDeIngreso { get; set; }

        [Required]
        public bool EsTemporal { get; set; }
        public DateTime FechaDeValidez { get; set; }
        public int MotorizadoId { get; set; }
        public bool EstadoDeMotocicleta { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Estado")]
        public short EstadoDelMotorizado { get; set; }

        public string EstadoMotorizadoClass { get; set; }

        [Required]
        [Range(typeof(bool), "true", "True", ErrorMessage = "Debe Aceptar los Terminos y Condiciones")]
        public bool AceptaTerminos { get; set; }

        [NotMapped]
        public string Nombres { get { return PrimerNombre + " " + PrimerApellido + " " + SegundoApellido; } }
    }

    public class AsignacionMotorizadoViewModel
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electronico")]
        public string CorreoElectronico { get; set; }

        public byte[] Foto { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Estado")]
        public short EstadoDelMotorizado { get; set; }

        public string EstadoMotorizadoClass { get; set; }

        public string EstadoMotorizadoDescripcion { get; set; }

        [NotMapped]
        public string Nombres { get; set; }


            }


    public class AsistenteViewModel
    {
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [Display(Name = "Estado de Asistente")]
        public bool EstadoDeAsistente { get; set; }

        [Display(Name = "Fecha de Baja")]
        public DateTime FechaDeBaja { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electronico")]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Primer Nombre")]
        public string PrimerNombre { get; set; }

        [StringLength(30)]
        [Display(Name = "Segundo Nombre")]
        public string SegundoNombre { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        public byte[] Foto { get; set; }

        [Required]
        [StringLength(9, ErrorMessage = "Excedio la Longitud Permitida")]//Masculino-Femenino
        public string Sexo { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [StringLength(16)]//132-090994-2000F
        public string Cedula { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

    }
}
