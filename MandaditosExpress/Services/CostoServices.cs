using MandaditosExpress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MandaditosExpress.Services
{
    public class CostoServices
    {

        private MandaditosDB db;

        public CostoServices(MandaditosDB db)
        {
            this.db = db;
        }
        public object ValidarVigenciaCostos(int TipoDeServicioId, DateTime FechaDeLaCotizacion, decimal MontoDeDinero)
        {
            //verificar que haya un costo vigente para el tipo de servicio dado
            var CostoAsociado = new object();
            CostoAsociado = db.Costos.FirstOrDefault(it => it.TipoDeServicioId == TipoDeServicioId && it.FechaDeFin >= FechaDeLaCotizacion && it.EstadoDelCosto);

            if (CostoAsociado == null)
            {
                CostoAsociado = db.CostoGestionBancaria.FirstOrDefault(cb=> cb.TipoDeServicioId == TipoDeServicioId &&
                                       cb.Estado && cb.FechaDeInicio <= FechaDeLaCotizacion &&
                                       cb.FechaDeFin >= FechaDeLaCotizacion &&
                                       MontoDeDinero >= cb.MontoDesde &&
                                       MontoDeDinero <= cb.MontoHasta
                                       );
            }

            return CostoAsociado;
        }
    }
}