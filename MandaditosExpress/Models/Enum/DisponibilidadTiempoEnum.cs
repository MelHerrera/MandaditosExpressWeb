using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models.Enum
{
    public enum DisponibilidadTiempoEnum
    {
        [Display(Name ="Mis Horas Libres")]
        HorasLibres=0,
        [Display(Name = "Medio Tiempo")]
        MedioTiempo =1,
        [Display(Name = "Tiempo Completo")]
        TiempoCompleto =2
    }
}