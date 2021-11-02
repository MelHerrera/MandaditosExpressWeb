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
        public object ValidarVigenciaCostos(int TipoDeServicioId, DateTime Fecha)
        {
            //verificar que haya un costo vigente para el tipo de servicio dado
            var CostoAsociado = new object();
            CostoAsociado = db.Costos.FirstOrDefault(it => it.TipoDeServicioId == TipoDeServicioId && it.FechaDeFin >= Fecha && it.EstadoDelCosto);

            if (CostoAsociado == null)
                CostoAsociado = db.CostoGestionBancaria.FirstOrDefault(it => it.TipoDeServicioId == TipoDeServicioId && it.FechaDeFin >= Fecha && it.Estado);

            return CostoAsociado;
        }

    }
}