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
        public int PersonaId { get; set; }
    }

    public partial class ResponseWsCredito
    {
        public int Id { get; set; }

        public string CodigoDelCredito { get; set; }

        public string Descripcion { get; set; }

        public string FechaDeInicio { get; set; }

        public string FechaDeVencimiento { get; set; }

        public double MontoDelCredito { get; set; }

        public string FechaDeCancelacion { get; set; }

        public int ClienteId { get; set; }

        public string ClienteNombres { get; set; }
    }
    public class ResponseWsPago
    {
        public int Id { get; set; }

        public string NumeroDePago { get; set; }

        public String FechaDePago { get; set; }

        public double MontoDelPago { get; set; }

        public double Cambio { get; set; }

        public string MonedaDescripcion { get; set; }

        public string TipoDePagoDescripcion { get; set; }

        public string EnvioCodigo { get; set; }

        public string CreditoCodigo { get; set; }

        public string ConceptoDelPago { get; set; }

        public string ClienteNombres { get; set; }

    }

    public partial class ResponseWsCotizacion
    {
        public int Id { get; set; }

        public string DescripcionDeCotizacion { get; set; }

        public string CodigoDeCotizacion { get; set; }

        public DateTime FechaDeLaCotizacion { get; set; }

        public DateTime FechaDeValidez { get; set; }

        public string LugarDestino { get; set; }

        public float DistanciaOrigenDestino { get; set; }
        public decimal MontoDeDinero { get; set; }

        public bool EsEspecial { get; set; }

        public decimal MontoTotal { get; set; }

        public int ClienteId { get; set; }
        public string ClienteNombres { get; set; }

        public int TipoDeServicioId { get; set; }
        public string TipoDeServicioDescripción { get; set; }

    }

    public partial class ResponseWsEnvio
    {
        public int Id { get; set; }

        public string CodigoDeEnvio { get; set; }

        public string DescripcionDeEnvio { get; set; }

        public DateTime FechaDelEnvio { get; set; }

        public double DistanciaEntregaRecep { get; set; }

        public decimal MontoDeDinero { get; set; }

        public decimal MontoTotalDelEnvio { get; set; }

        public bool EsUrgente { get; set; }

        public string EstadoDelEnvioText { get; set; }

        public bool Finalizado { get; set; }
        public bool Rechazado { get; set; }

        public bool EsAlCredito { get; set; }
        public bool EstaRetrasado { get; set; }

        public double TiempoRetraso { get; set; }

        public string MotivoDeRechazo { get; set; }
        public string Cliente { get; set; }

        public string LugarOrigen { get; set; }

        public bool Asignado { get; set; }
        public string NombresDelMotorizado { get; set; }
    }

    public partial class ResponseWsTipoDeServicio {
        public int Id { get; set; }
        public string DescripcionTipoDeServicio { get; set; }
        public bool EstadoTipoDeServicio { get; set; }
    }

    public partial class ResponseWsListaDeCreditos
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }

        public List<ResponseWsCredito> Creditos { get; set; }
    }

    public partial class ResponseWsListaDePagos
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }

        public List<ResponseWsPago> Pagos { get; set; }
    }

    public partial class ResponseWsListaDeCotizaciones
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }

        public List<ResponseWsCotizacion> Cotizaciones { get; set; }
    }

    public partial class ResponseWsListaTiposDeServicio
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }

        public List<ResponseWsTipoDeServicio> TiposDeServicio { get; set; }
    }

    public partial class ResponseWsListaEnvios
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }

        public List<ResponseWsEnvio> Envios { get; set; }
    }

    public partial class ResponseWsImagenPerfil
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }

        public byte[] Imagen { get; set; }
    }
}