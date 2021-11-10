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
    }
}