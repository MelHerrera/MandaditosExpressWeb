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

        public string ValidarFechaCreate(DateTime FIncicio, DateTime Ffin, int TipoDeServicioId)
        {
            // fecha de inicio no puede ser menor por mas de 3 minutos a la fecha actual
            // fecha inicio no puede ser mayor o igual a la fecha de fin
            // fecha fin no puede ser menor o igual a la fecha de inicio
            // fecha inicio no puede ser menos a una de las fechas de inicio de los costos vigentes para el mismo tipo de servicio
            // la fecha fin no puede ser menor a una de las fechas de inicio de los costos vigentes para el mismo tipo de servicio
            // la fecha fin no puede ser menor o igual a una de las fechas de fin de los costos vigentes para el mismo tipo de servicio

            var error = string.Empty;

            if (FIncicio.AddMinutes(3) < DateTime.Now)//permitirle que la fecha de inicio solo pueda ser 3 minutos antes de la hora y fecha actual
                return error = "La fecha de inicio debe ser igual o mayor  a la fecha y hora actual. No puede iniciar un costo en un instante de tiempo pasado";
            if (FIncicio >= Ffin)
                return error = "La fecha de inicio no puede ser mayor o igual a la fecha de finalización del costo";
            if (Ffin <= FIncicio)
                return error = "La fecha de fin no puede ser menor o igual a la fecha de inicio del costo";

            var CostosAntiguo = (from c in db.Costos
                                 where c.TipoDeServicioId == TipoDeServicioId && c.EstadoDelCosto
                                 select c).ToList();

            if (CostosAntiguo.Any(it => it.TipoDeServicioId == TipoDeServicioId && it.EstadoDelCosto && FIncicio <= it.FechaDeInicio))
                return error = "No se puede agregar, ya existe un costo(s) vigentes para este periodo";

            if (CostosAntiguo.Any(it => it.TipoDeServicioId == TipoDeServicioId && it.EstadoDelCosto && Ffin <= it.FechaDeInicio))
                return error = "No se puede agregar, ya existe un costo(s) vigentes para este periodo";

            if (CostosAntiguo.Any(it => it.TipoDeServicioId == TipoDeServicioId && it.EstadoDelCosto && Ffin <= it.FechaDeFin))
                return error = "No se puede agregar, ya existe un costo(s) vigentes para este periodo";

            return error;
        }

        public string validateFechaEdit(Costo costo)
        {
            if (costo.FechaDeInicio >= costo.FechaDeFin)
                return "La fecha de inicio no puede ser mayor o igual a la fecha de finalización del costo";
            if (costo.FechaDeFin <= costo.FechaDeInicio)
                return "La fecha de fin no puede ser menor o igual a la fecha de inicio del costo";

            if (costo.EstadoDelCosto == false)//si se esta queriendo desactivar el costo verificar que haya uno el cual la fecha de fin sea mayor a la fecha y hora actual
            {
                //cosotos vigentes asociados
                var CostosAntiguo = (from c in db.Costos
                                     where c.TipoDeServicioId == costo.TipoDeServicioId && c.EstadoDelCosto && c.FechaDeFin > DateTime.Now
                                     select c).ToList();

                if (CostosAntiguo.Count == 1)//si hay algun costo activo y vigente entonces validar, si solo hay uno no hacer nada ya que, si se desactiva no quedarian costos vigentes
                        return "No se puede inhabilitar el costo debido a que no quedaria ningun costo vigente";

                if (CostosAntiguo.Count > 1)//si hay algun costo activo y vigente entonces validar, si solo hay uno no hacer nada ya que, si se desactiva no quedarian costos vigentes
                {
                    var ExisteVigente = false;

                    if (CostosAntiguo.Any(it => it.FechaDeFin > DateTime.Now))//si todos los elementos tienen una fecha de fin menos a la fecha actual entonces no quedaria ningun costo vigente
                        ExisteVigente = true;

                    if(!ExisteVigente)
                        return "No se puede inhabilitar el costo debido a que no quedaria ningun costo vigente";
                }
            }

            return "";
        }
    }
}