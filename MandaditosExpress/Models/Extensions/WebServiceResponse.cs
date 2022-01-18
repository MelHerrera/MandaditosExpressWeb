using System;
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

    public partial class ResponseWsCredito
    {
        public int Id { get; set; }

        public string CodigoDelCredito { get; set; }

        public string Descripcion { get; set; }

        public string FechaDeInicio { get; set; }

        public string FechaDeVencimiento { get; set; }

        public bool EstadoDelCredito { get; set; }

        public string FechaDeCancelacion { get; set; }

        public int ClienteId { get; set; }

        public string ClienteNombres { get; set; }
    }

    public partial class ResponseWsListaDeCreditos
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }

        public List<ResponseWsCredito> Creditos { get; set; }
    }
}