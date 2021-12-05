
using System;

namespace MandaditosExpress.Services
{
    public static class Utilidades
    {
        public static int MinGestionBancaria = 120;
        public static int MaxGestionBancaria = 10000;




        public static string GenerateDefaultPasswordByEmail(string Email)
        {
            //default password will be : micorreo@gmail.com#2021   ---- 'correo' + '#' + 'M' + 'año actual'
            return Email + "#" + "M" + DateTime.Now.Year.ToString();
        }
    }
}