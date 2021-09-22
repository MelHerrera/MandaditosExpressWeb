using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace MandaditosExpress
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ////globalizacion y localizacion
            //ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            //ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeBinder());
            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-NI");
            //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-NI");
        }
    }
}
