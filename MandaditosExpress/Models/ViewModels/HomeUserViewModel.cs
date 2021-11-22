using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MandaditosExpress.Models.ViewModels
{
    public class HomeUserViewModel
    {
        public int EnviosMensuales { get; set; }
        public int EnviosAnuales { get; set; }
        public int EnviosDelDia { get; set; }
        public int TodosEnvios { get; set; }
        public int CreditosPendientes { get; set; }

        public int EnviosRealizados { get; set; }
        public int EnviosRechazados { get; set; }

        public ICollection<EnvioHistorialViewModel> EnviosHistorial { get; set; }
    }
}