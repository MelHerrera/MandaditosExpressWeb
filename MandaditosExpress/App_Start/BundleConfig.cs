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
                      "~/Scripts/knockout/knockout-3.5.1.js",
                      "~/Scripts/knockout/extensions/CustomBindins.js"));

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
                      "~/Scripts/plugins/chartjs/chart.min.js"));


            // sweet Alert 2
            bundles.Add(new ScriptBundle("~/plugins/sweetalert").Include(
                      "~/Scripts/plugins/sweetalert/sweetalert2.all.min.js"));

            // notify bootstrap
            bundles.Add(new ScriptBundle("~/plugins/notify").Include(
                      "~/Scripts/plugins/notify/bootstrap-notify.min.js"));

            // Peity
            bundles.Add(new ScriptBundle("~/plugins/peity").Include(
                      "~/Scripts/plugins/peity/jquery.peity.min.js"));

            // Custom switches
            bundles.Add(new ScriptBundle("~/plugins/switch").Include(
                      "~/Scripts/plugins/switch/bootstrap-switch-button.min.js"));

            // Location Picker jquery
            bundles.Add(new ScriptBundle("~/plugins/locationpicker").Include(
                      "~/Scripts/plugins/locationpicker/locationpicker.jquery.js"));

            // dataTables scripts
            bundles.Add(new ScriptBundle("~/plugins/dataTables").Include(
                       "~/Scripts/plugins/dataTables/datatables.min.js",
                      "~/Scripts/plugins/dataTables/dataTables.bootstrap4.min.js"));

            // WOW 
            bundles.Add(new ScriptBundle("~/plugins/wow").Include(
                        "~/Scripts/plugins/wow/wow.min.js"));

            // IndexMonedas
            bundles.Add(new ScriptBundle("~/bundles/IndexMonedas").Include(
                        "~/Scripts/knockout/Componentes/ModalComponent.js",
                        "~/Scripts/knockout/extensions/CustomBindins.js",
                        "~/Scripts/knockout/ViewModel/Moneda/MonedaViewModel.js",
                        "~/Scripts/knockout/ViewModel/Moneda/IndexMoneda.js"));

             // IndexDisponibilidad
            bundles.Add(new ScriptBundle("~/bundles/IndexDisponibilidad").Include(
                        "~/Scripts/knockout/Componentes/ModalComponent.js",
                        "~/Scripts/knockout/extensions/CustomBindins.js",
                        "~/Scripts/knockout/ViewModel/Disponibilidad/DisponibilidadViewModel.js",
                        "~/Scripts/knockout/ViewModel/Disponibilidad/IndexDisponibilidad.js"));

            // IndexVelocidad
            bundles.Add(new ScriptBundle("~/bundles/IndexVelocidadDeConexion").Include(
                        "~/Scripts/knockout/Componentes/ModalComponent.js",
                        "~/Scripts/knockout/extensions/CustomBindins.js",
                        "~/Scripts/knockout/ViewModel/VelocidadDeConexion/VelocidadDeConexionViewModel.js",
                        "~/Scripts/knockout/ViewModel/VelocidadDeConexion/IndexVelocidadDeConexion.js"));

            // IndexTipoDeServicio
            bundles.Add(new ScriptBundle("~/bundles/IndexTipoDeServicio").Include(
                        "~/Scripts/knockout/Componentes/ModalComponent.js",
                        "~/Scripts/knockout/extensions/CustomBindins.js",
                        "~/Scripts/knockout/ViewModel/TipoDeServicio/TipoDeServicioViewModel.js",
                        "~/Scripts/knockout/ViewModel/TipoDeServicio/IndexTipoDeServicio.js"));

            // Indexcosto
            bundles.Add(new ScriptBundle("~/bundles/IndexCosto").Include(
                        "~/Scripts/knockout/Componentes/ModalComponent.js",
                        "~/Scripts/knockout/extensions/CustomBindins.js",
                        "~/Scripts/knockout/ViewModel/Costo/CostoViewModel.js",
                        "~/Scripts/knockout/ViewModel/Costo/IndexCosto.js"));

            // IndexCostoGestionBancaria
            bundles.Add(new ScriptBundle("~/bundles/IndexCostoGestionBancaria").Include(
                        "~/Scripts/knockout/Componentes/ModalComponent.js",
                        "~/Scripts/knockout/extensions/CustomBindins.js",
                        "~/Scripts/knockout/ViewModel/CostosGestionBancaria/CostoGestionBancariaViewModel.js",
                        "~/Scripts/knockout/ViewModel/CostosGestionBancaria/IndexCostoGestionBancaria.js"));

            // IndexTipoDeServicio
            bundles.Add(new ScriptBundle("~/bundles/IndexServicio").Include(
                        "~/Scripts/knockout/Componentes/ModalComponent.js",
                        "~/Scripts/knockout/extensions/CustomBindins.js",
                        "~/Scripts/knockout/ViewModel/Servicio/ServicioViewModel.js",
                        "~/Scripts/knockout/ViewModel/Servicio/IndexServicio.js"));


            // IndexTipoDePagos
            bundles.Add(new ScriptBundle("~/bundles/IndexTipoDePagos").Include(
                        "~/Scripts/knockout/Componentes/ModalComponent.js",
                        "~/Scripts/knockout/extensions/CustomBindins.js",
                        "~/Scripts/knockout/ViewModel/TiposDePago/TipoDePagoViewModel.js",
                        "~/Scripts/knockout/ViewModel/TiposDePago/IndexTipoDePago.js"));

            // IndexCreditos
            bundles.Add(new ScriptBundle("~/bundles/IndexCreditos").Include(
                        "~/Scripts/knockout/Componentes/ModalComponent.js",
                        "~/Scripts/knockout/extensions/CustomBindins.js",
                        "~/Scripts/knockout/ViewModel/Creditos/CreditoViewModel.js",
                        "~/Scripts/knockout/ViewModel/Creditos/IndexCredito.js"));

            //Styles
            //datapicker
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                "~/Scripts/plugins/datetimepicker/bootstrap-datetimepicker.min.js"
           ));


            //Styles

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/animate.css"));

            bundles.Add(new StyleBundle("~/Content/landing").Include(
                "~/Content/landing_style.css"));

            bundles.Add(new StyleBundle("~/Content/site/login").Include(
                  "~/Content/site_login.css"));

            bundles.Add(new StyleBundle("~/Content/site/cotizacion").Include(
      "~/Content/site_cotizacion.css"));


            bundles.Add(new StyleBundle("~/Content/site/register").Include(
                  "~/Content/register_style.css"));

            bundles.Add(new StyleBundle("~/Content/site/forgotpass").Include(
                "~/Content/forgot_pass.css"));

            bundles.Add(new StyleBundle("~/Content/site/confirmarcorreo").Include(
                "~/Content/confirm_correo.css"));


            bundles.Add(new StyleBundle("~/Content/site/resetpass").Include(
                "~/Content/reset_password.css"));

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
            bundles.Add(new StyleBundle("~/Plugin/switches").Include(
                      "~/Content/plugins/switch/bootstrap-switch-button.min.css"));
            //timepicker
            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include(
                  "~/Content/plugins/datetimepicker/bootstrap-datetimepicker.min.css"
                  ));
        }
    }
}
