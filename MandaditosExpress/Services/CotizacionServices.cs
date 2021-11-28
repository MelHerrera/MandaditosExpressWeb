using MandaditosExpress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MandaditosExpress.Services
{
    public class CotizacionServices
    {
        private MandaditosDB db;

        public CotizacionServices(MandaditosDB db)
        {
            this.db = db;
        }

        public decimal Cotizar(int TipoDeServicioId, DateTime FechaDeLaCotizacion, decimal MontoDeDinero, float DistanciaOrigenDestino, bool EsUrgente)
        {
            var CostoTotal = 0.0M;
            //obtener el costo asociado al tipo de servicio pero que este activo y en vigencia.
            var CostoAsociado = db.Costos.DefaultIfEmpty(null).FirstOrDefault(x => (x.TipoDeServicioId == TipoDeServicioId && x.EstadoDelCosto && x.FechaDeFin > FechaDeLaCotizacion));

            if (CostoAsociado != null && MontoDeDinero <= 0 && DistanciaOrigenDestino > 0)
            {
                CostoTotal = (decimal)(CostoAsociado.CostoDeAsistencia + CostoAsociado.CostoDeGasolina + CostoAsociado.CostoDeMotorizado +
                ((CostoAsociado.DistanciaBase + DistanciaOrigenDestino) * CostoAsociado.PrecioPorKm));

                if (EsUrgente)
                    CostoTotal += (decimal)CostoAsociado.PrecioDeRecargo;
            }
            else
            {
                var CostoGestion = db.CostoGestionBancaria.DefaultIfEmpty(null).FirstOrDefault(x => (x.TipoDeServicioId == TipoDeServicioId && x.Estado && x.FechaDeFin > FechaDeLaCotizacion));

                if (CostoGestion != null)
                {
                    var CostoPorcentaje = (from cb in db.CostoGestionBancaria
                                           where cb.TipoDeServicioId == TipoDeServicioId &&
                                           cb.Estado && cb.FechaDeInicio < FechaDeLaCotizacion &&
                                           cb.FechaDeFin > FechaDeLaCotizacion &&
                                           MontoDeDinero >= cb.MontoDesde &&
                                           MontoDeDinero <= cb.MontoHasta
                                           select cb);

                    //se cobra un porcentaje o un valor directamente si asi se definio en el costo
                    var Porcentaje = CostoPorcentaje.Count() > 0 ? CostoPorcentaje.First().Porcentaje : 0;

                    if (Porcentaje > 0 && MontoDeDinero > 0)
                        CostoTotal = MontoDeDinero * ((decimal)(Porcentaje / 100));
                    else
                    {
                        var valor = CostoPorcentaje.Count() > 0 ? CostoPorcentaje.First().Valor : 0;

                        if (valor > 0 && MontoDeDinero > 0)
                            CostoTotal = valor;
                    }

                    if (EsUrgente)
                        CostoTotal += (decimal)CostoGestion.PrecioDeRecargo;
                }
            };

            return CostoTotal;

        }


        public string ValidarDatosCotizacion(int TipoDeServicioId, float Distancia, decimal MontoDinero, DateTime FechaDelEnvio)
        {
            string message = "";

            var costoAsociado = new object();

            costoAsociado = db.Costos.FirstOrDefault(it => it.TipoDeServicioId == TipoDeServicioId && it.FechaDeFin >= FechaDelEnvio && it.EstadoDelCosto);

            if (costoAsociado != null) // si se encontro un costo asociado en costos significa que es cualquier tipo de servicio que no sea Gestiones Bancarias
            {
                if (Distancia <= 0)
                    message = "Estimado cliente la distancia debe ser Mayor a 0 Km";
            }
            else
            {
                costoAsociado = db.CostoGestionBancaria.FirstOrDefault(it => it.TipoDeServicioId == TipoDeServicioId && it.FechaDeFin >= FechaDelEnvio && it.Estado);

                if (costoAsociado != null) //si se encontro un costo en costos de gestion bancarias significa que el tipo de servicio es gestiones bancarias
                {
                    if (MontoDinero < Utilidades.MinGestionBancaria || MontoDinero > Utilidades.MaxGestionBancaria)
                        message = String.Format("Estimado cliente por razones de seguridad actualmente solo realizamos gestiones bancarias con montos de {0} a {1}, para una cantidad diferente contactese con el negocio. ", Utilidades.MinGestionBancaria, Utilidades.MaxGestionBancaria);
                }
            }

            //en este punto regresara un mensaje de error con respecto al tipo de servicio o vacio si se proporcionaron datos correctos o en el sistema aun no hay ningun costo asociado a los tipos de servicios ofrecidos
            return message;
        }

    }
}