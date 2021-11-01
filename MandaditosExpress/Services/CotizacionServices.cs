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
                    var Porcentaje = CostoPorcentaje.Count() > 0 ? CostoPorcentaje.First().Porcentaje : 0;

                    if (Porcentaje > 0 && MontoDeDinero > 0)
                        CostoTotal = MontoDeDinero * ((decimal)(Porcentaje / 100));

                    if (EsUrgente)
                        CostoTotal += (decimal)CostoGestion.PrecioDeRecargo;
                }
            };

            return CostoTotal;

        }
    }
}