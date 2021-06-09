using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MandaditosExpress.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }
        public short NumeroDePago { get; set; }
        public DateTime FechaDePago { get; set; }
        public double MontoADescontar { get; set; }
        public double Cambio { get; set; }
        public double CambioDolar { get; set; }
        public int MonedaId { get; set; }
        public int TipoDePagoId { get; set; }
        public int EnvioId { get; set; }
        public bool EstadoDelPago { get; set; }

        public virtual Moneda Moneda { get; set; }
        public virtual TipoDePago TipoDePago { get; set; }
        public virtual Envio Envio { get; set; }
    }
}