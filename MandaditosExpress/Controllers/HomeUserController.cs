using MandaditosExpress.Models;
using MandaditosExpress.Models.Utileria;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MandaditosExpress.Controllers
{
    [Authorize]
    public class HomeUserController : Controller
    {
        private MandaditosDB db = new MandaditosDB();

        // GET: HomeUser
        public ActionResult Index()
        {
            var UserName = Request.GetOwinContext().Authentication.User.Identity.Name;
            ViewBag.PersonaActual = new Utileria().BuscarPersonaPorUsuario(UserName);
            return View();
        }
    }
}