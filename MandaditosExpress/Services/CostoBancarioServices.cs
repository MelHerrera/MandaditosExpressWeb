using MandaditosExpress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MandaditosExpress.Services
{
    public class CostoBancarioServices
    {
        private MandaditosDB db;
        public CostoBancarioServices(MandaditosDB db)
        {
            this.db = db;
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

            var CostosAntiguo = (from c in db.CostoGestionBancaria
                                 where c.TipoDeServicioId == TipoDeServicioId && c.Estado
                                 select c).ToList();

            if (CostosAntiguo.Any(it => it.TipoDeServicioId == TipoDeServicioId && it.Estado&& FIncicio <= it.FechaDeInicio))
                return error = "No se puede agregar, ya existe un costo(s) vigentes para este periodo";

            if (CostosAntiguo.Any(it => it.TipoDeServicioId == TipoDeServicioId && it.Estado && Ffin <= it.FechaDeInicio))
                return error = "No se puede agregar, ya existe un costo(s) vigentes para este periodo";

            if (CostosAntiguo.Any(it => it.TipoDeServicioId == TipoDeServicioId && it.Estado && Ffin <= it.FechaDeFin))
                return error = "No se puede agregar, ya existe un costo(s) vigentes para este periodo";

            return error;
        }

        public string validateFechaEdit(CostoGestionBancaria costo)
        {
            if (costo.Porcentaje <= 0 && costo.Valor <= 0)
                return "Debe especificar el porcentaje a o el valor a cobrar por la gestion bancaria";

            if (costo.Porcentaje > 0 && costo.Valor > 0)
                return "No puede especificar el porcentaje y valor a la vez, solo se debe especificar uno de ellos.";

            if (costo.FechaDeInicio >= costo.FechaDeFin)
                return "La fecha de inicio no puede ser mayor o igual a la fecha de finalización del costo";
            if (costo.FechaDeFin <= costo.FechaDeInicio)
                return "La fecha de fin no puede ser menor o igual a la fecha de inicio del costo";

            if (costo.Estado == false)//si se esta queriendo desactivar el costo verificar que haya uno el cual la fecha de fin sea mayor a la fecha y hora actual
            {
                //cosotos vigentes asociados
                var CostosAntiguo = (from c in db.CostoGestionBancaria
                                     where c.TipoDeServicioId == costo.TipoDeServicioId && c.Estado && c.FechaDeFin > DateTime.Now
                                     select c).ToList();

                if (CostosAntiguo.Count == 1)//si hay algun costo activo y vigente entonces validar, si solo hay uno no hacer nada ya que, si se desactiva no quedarian costos vigentes
                    return "No se puede inhabilitar el costo debido a que no quedaria ningun costo vigente";

                if (CostosAntiguo.Count > 1)//si hay algun costo activo y vigente entonces validar, si solo hay uno no hacer nada ya que, si se desactiva no quedarian costos vigentes
                {
                    var ExisteVigente = false;

                    if (CostosAntiguo.Any(it => it.FechaDeFin > DateTime.Now))//si todos los elementos tienen una fecha de fin menos a la fecha actual entonces no quedaria ningun costo vigente
                        ExisteVigente = true;

                    if (!ExisteVigente)
                        return "No se puede inhabilitar el costo debido a que no quedaria ningun costo vigente";
                }
            }

            var CostoInDb = db.CostoGestionBancaria.Find(costo.Id);

            if (CostoInDb != null)
            {
                if (costo.Estado && CostoInDb.Estado == false)//si se esta queriendo activar un costo entonces evitar que hayan 2 costos activos, si en la bd es false y en el enviado es true significa que se esta queriendo habilitar
                {
                    //costos activos y vigentes
                    var CostosAntiguo = (from c in db.CostoGestionBancaria
                                         where c.TipoDeServicioId == costo.TipoDeServicioId && c.Estado && c.FechaDeFin > DateTime.Now
                                         select c).ToList();

                    if (CostosAntiguo.Count >= 1)
                        return "No se puede habilitar porque solamente debe haber un costo activo";
                }
            }

            return "";
        }
    }
}