using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models.Enum
{
    public enum VelocidadInternetEnum
    {
        Lenta=0,
        Media=1,
        Rapida=2,
        [Display(Name ="Muy Rapida")]
        MuyRapida=3
    }
}