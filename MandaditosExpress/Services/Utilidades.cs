
using System;

using MandaditosExpress.Models;
using MandaditosExpress.Models.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MandaditosExpress.Services
{
    public static class Utilidades
    {
        public static int MinGestionBancaria = 120;
        public static int MaxGestionBancaria = 10000;




        public static string GenerateDefaultPasswordByEmail(string Email)
        {
            //default password will be : micorreo@gmail.com#M2021   ---- 'correo' + '#' + 'M' + 'año actual'
            return Email + "#" + "M" + DateTime.Now.Year.ToString();
        }
    }
}