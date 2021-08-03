using System.Linq;

namespace MandaditosExpress.Models.Utileria
{
    public class Utileria
    {
        private MandaditosDB db = new MandaditosDB();

        public Persona BuscarPersonaPorUsuario(string UserName)
        {
            return db.Personas.DefaultIfEmpty(null).FirstOrDefault(x => x.CorreoElectronico == UserName);
        }
    }
}