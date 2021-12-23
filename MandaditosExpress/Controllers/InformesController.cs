using System.Web.Mvc;
using MandaditosExpress.Models.DataSets.FltListCltsFechaTableAdapters;
using System;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using MandaditosExpress.Models.DataSets.Ds_EnviosCredClienteXFechaTableAdapters;
using MandaditosExpress.Models;
using MandaditosExpress.Models.DataSets.Ds_EnviosMensualesTableAdapters;
using MandaditosExpress.Models.DataSets.DS_EnviosPorPeriodoTableAdapters;
using System.Reflection;

namespace MandaditosExpress.Controllers
{
    public class InformesController : Controller
    {
        private MandaditosDB db = new MandaditosDB();
        // GET: Informes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListClientesxFec( DateTime Desde, DateTime Hasta)
        {
            FltListCltsFechTableAdapter dt = new FltListCltsFechTableAdapter();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes/RptFiltrarClientesPorFecha.rdlc";
            rpt.LocalReport.DataSources.Add(new ReportDataSource("Ds_InformeNueCli", dt.GetData(Desde, Hasta).ToList()));
            rpt.LocalReport.SetParameters(new ReportParameter("Usuario", Request.GetOwinContext().Authentication.User.Identity.Name));//para poder mostrar el usuario que genereo el reporte
            rpt.LocalReport.SetParameters(new ReportParameter("Desde", Desde.ToString()));//para poder mostrar el periodo en el reporte
            rpt.LocalReport.SetParameters(new ReportParameter("Hasta", Hasta.ToString() ));//para poder mostrar el periodo en el reporte
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            return View();

        }

        public ActionResult ListaEnviosPorPeriodo(DateTime Desde, DateTime Hasta)
        {
            FltEnviosPorPeriodoTableAdapter dt = new FltEnviosPorPeriodoTableAdapter();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes/RptEnviosPorPeriodo.rdlc";
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DsInfo_EnviosPorPeriodo", dt.GetData(Desde, Hasta).ToList()));
            rpt.LocalReport.SetParameters(new ReportParameter("Usuario", Request.GetOwinContext().Authentication.User.Identity.Name));//para poder mostrar el usuario que genereo el reporte
            rpt.LocalReport.SetParameters(new ReportParameter("Desde", Desde.ToString()));//para poder mostrar el periodo en el reporte
            rpt.LocalReport.SetParameters(new ReportParameter("Hasta", Hasta.ToString()));//para poder mostrar el periodo en el reporte
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            return View();

        }

        public ActionResult EnviosCredClienteFecha(DateTime Desde, DateTime Hasta, int ClienteId)
        {
            var cliente = db.Personas.Find(ClienteId);

            FltEnviosCredXfechaYClienteTableAdapter dt = new FltEnviosCredXfechaYClienteTableAdapter();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes/RptEnviosCredXClienteYFech.rdlc";
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DataSet_EnviosCredClienteXFecha", dt.GetData(Desde, Hasta, ClienteId).ToList()));
            rpt.LocalReport.SetParameters(new ReportParameter("Usuario", Request.GetOwinContext().Authentication.User.Identity.Name));//para poder mostrar el usuario que genereo el reporte
            rpt.LocalReport.SetParameters(new ReportParameter("Desde", Desde.ToString()));//para poder mostrar el periodo en el reporte
            rpt.LocalReport.SetParameters(new ReportParameter("Hasta", Hasta.ToString()));//para poder mostrar el periodo en el reporte
            rpt.LocalReport.SetParameters(new ReportParameter("Cliente", cliente.NombreCompleto));//para poder mostrar el cliente en el reporte
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;

            return View();
        }

        public ActionResult EnviosMensuales()
        {
            FltEnviosMensualesTableAdapter dt = new FltEnviosMensualesTableAdapter();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes/RptEnviosMensuales.rdlc";
            rpt.LocalReport.DataSources.Add(new ReportDataSource("DsInf_EnviosMensuales", dt.GetData().ToList()));
            rpt.LocalReport.SetParameters(new ReportParameter("Usuario", Request.GetOwinContext().Authentication.User.Identity.Name));//para poder mostrar el usuario que genereo el reporte
            rpt.LocalReport.SetParameters(new ReportParameter("Mes", DateTime.Now.ToString("MMMM") ));//para poder mostrar el usuario que genereo el reporte
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;

            return View();
        }

        public ActionResult ImprimirCotizacion(DateTime Fecha, DateTime FechaDeValidez, float Monto,int TipoDeServicioId, string Descripcion, double MontoDeDinero, string Origen, string Destino, float Distancia=0, bool Urgente=false)
        {
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes/RptCotizacion.rdlc";

            var TipoDeServicio = db.TiposDeServicio.Find(TipoDeServicioId);
            rpt.LocalReport.SetParameters(new ReportParameter("TipoDeServicio", TipoDeServicio!=null ? TipoDeServicio.DescripcionTipoDeServicio : "-- Sin Especificar --"));//para poder mostrar el usuario que genereo el reporte
            rpt.LocalReport.SetParameters(new ReportParameter("Fecha", Fecha.ToString()));
            rpt.LocalReport.SetParameters(new ReportParameter("FechaDeValidez", FechaDeValidez.ToString()));
            rpt.LocalReport.SetParameters(new ReportParameter("Descripcion", Descripcion.Trim()));
            rpt.LocalReport.SetParameters(new ReportParameter("Origen", Origen.Length > 0 ? Origen : "-- Sin Especificar --"));
            rpt.LocalReport.SetParameters(new ReportParameter("Destino", Destino.Length>0 ? Destino: "-- Sin Especificar --"));
            rpt.LocalReport.SetParameters(new ReportParameter("MontoDeDinero", MontoDeDinero.ToString()));
            rpt.LocalReport.SetParameters(new ReportParameter("Distancia", Distancia.ToString()));
            rpt.LocalReport.SetParameters(new ReportParameter("Urgente", Urgente ? "Si" : "No"));
            rpt.LocalReport.SetParameters(new ReportParameter("Monto", Monto.ToString()));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            return View();

        }
    }
}