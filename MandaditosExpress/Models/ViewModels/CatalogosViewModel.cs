using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models.ViewModels
{
    public class TipoDeServicioViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        [Display(Name = "Descripción")]
        public string DescripcionTipoDeServicio { get; set; }

        [Display(Name = "Estado")]
        public bool EstadoTipoDeServicio { get; set; }
    }

    public class ServicioViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(200)]
        [Display(Name = "Descripción")]
        public string DescripcionDelServicio { get; set; }

        public bool Estado { get; set; }

        [Required]
        public int TipoDeServicioId { get; set; }

    }

    public class TipoDePagoViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool EstadoTipoDePago { get; set; }

    }

    public class LugarViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public string Direccion { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }
    }

}