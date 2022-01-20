using MandaditosExpress.Models;
using MandaditosExpress.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MandaditosExpress.Services
{
    public class MotorizadoServices
    {

        private MandaditosDB db;

        public MotorizadoServices(MandaditosDB db)
        {
            this.db = db;
        }

        public bool CambiarEstadoMotorizado(EstadoDeMotorizadoEnum EstadoMotorizado, Motorizado Motorizado)
        {
            Motorizado.EstadoDelMotorizado = (short)EstadoMotorizado;
            db.Entry(Motorizado).State = EntityState.Modified;

            if(db.SaveChanges()>0)
            return true;

            return false;
        }
    }
}