using System.Web.Mvc;
using MandaditosExpress.Models.DataSets.FltListCltsFechaTableAdapters;
using System;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace MandaditosExpress.Controllers
{
    public class InformesController : Controller
    {
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


    }
}