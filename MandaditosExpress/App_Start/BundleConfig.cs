using System.Web;
using System.Web.Optimization;

namespace MandaditosExpress
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //Knockout
            bundles.Add(new ScriptBundle("~/plugins/knockout").Include(
                      "~/Scripts/knockout/knockout-3.5.1.js"));

            //JqueryUi
            bundles.Add(new ScriptBundle("~/plugins/jqueryui").Include(
                      "~/Scripts/plugins/jquery-ui/jquery-ui.min.js"));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include(
                      "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js"));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/plugins/moment").Include(
                      "~/Scripts/plugins/moment/moment.js"));

            // Js Color Picker
            bundles.Add(new ScriptBundle("~/plugins/jscolorpicker").Include(
                      "~/Scripts/plugins/jscolor/jscolor.js"));

            // Inspinia script
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                      "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                      "~/Scripts/plugins/pace/pace.min.js",
                      "~/Scripts/app/inspinia.js"));

            //Inspinia Landing Page 
            bundles.Add(new ScriptBundle("~/bundles/plugins/landing").Include(
                 "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                "~/Scripts/plugins/pace/pace.min.js",
                "~/Scripts/plugins/wow/wow.min.js",
                "~/Scripts/app/inspinia_landing.js",
                  "~/Scripts/app/inspinia.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/popper.min.js"));


            // Flot chart
            bundles.Add(new ScriptBundle("~/plugins/flot").Include(
                      "~/Scripts/plugins/flot/jquery.flot.js",
                      "~/Scripts/plugins/flot/jquery.flot.tooltip.min.js",
                      "~/Scripts/plugins/flot/jquery.flot.resize.js",
                      "~/Scripts/plugins/flot/jquery.flot.pie.js",
                      "~/Scripts/plugins/flot/jquery.flot.time.js",
                      "~/Scripts/plugins/flot/jquery.flot.spline.js"));

            // Sparkline
            bundles.Add(new ScriptBundle("~/plugins/sparkline").Include(
                      "~/Scripts/plugins/sparkline/jquery.sparkline.min.js"));

            // ChartJS chart
            bundles.Add(new ScriptBundle("~/plugins/chartJs").Include(
                      "~/Scripts/plugins/chartjs/Chart.min.js"));


            // sweet Alert 2
            bundles.Add(new ScriptBundle("~/plugins/sweetalert").Include(
                      "~/Scripts/plugins/sweetalert/sweetalert2.all.min.js"));

            // Peity
            bundles.Add(new ScriptBundle("~/plugins/peity").Include(
                      "~/Scripts/plugins/peity/jquery.peity.min.js"));

            // Custom switches
            bundles.Add(new ScriptBundle("~/plugins/switch").Include(
                      "~/Scripts/plugins/switch/bootstrap-switch-button.min.js"));

            // dataTables scripts
            bundles.Add(new ScriptBundle("~/plugins/dataTables").Include(
                       "~/Scripts/plugins/dataTables/datatables.min.js",
                      "~/Scripts/plugins/dataTables/dataTables.bootstrap4.min.js"));

            // WOW 
            bundles.Add(new ScriptBundle("~/plugins/wow").Include(
                        "~/Scripts/plugins/wow/wow.min.js"));


            //Styles

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
          "~/Content/bootstrap.min.css",
          "~/Content/animate.css",
          "~/Content/landing_style.css"));

            bundles.Add(new StyleBundle("~/Content/site/login").Include(
                  "~/Content/site_login.css"));


            bundles.Add(new StyleBundle("~/Content/site/register").Include(
                  "~/Content/register_style.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/animate.css",
                       "~/Content/style.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/font-awesome/css").Include(
                      "~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            // dataTables css styles
            bundles.Add(new StyleBundle("~/Content/dataTablesStyles").Include(
                      "~/Content/plugins/dataTables/dataTables.min.css"));

            // custom switches 
            bundles.Add(new StyleBundle("~/Plugind/switches").Include(
                      "~/Content/plugins/switch/bootstrap-switch-button.min.css"));

        }
    }
}
