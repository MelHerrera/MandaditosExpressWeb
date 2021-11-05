using System.Collections.Generic;

namespace MandaditosExpress.Models.Extensions
{
    public partial class RespuestaLogin
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }

        public bool IsConfirmed { get; set; }
        public List<string> Roles { get; set; }
    }
}