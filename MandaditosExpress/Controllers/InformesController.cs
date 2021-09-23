﻿using Microsoft.Reporting.WebForms;
using System.Web.Mvc;
using MandaditosExpress.Models.DataSets.Ds_InformeNueCliTableAdapters;
using System;
using System.Linq;

namespace MandaditosExpress.Controllers
{
    public class InformesController : Controller
    {
        // GET: Informes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListClientesxFec(/*int idCli,*/ string Desde = " ", string Hasta ="")
        {
            FltListCltsFechTableAdapter dt = new FltListCltsFechTableAdapter();
            ReportViewer rpt = new ReportViewer();
            rpt.ProcessingMode = ProcessingMode.Local;
            rpt.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reportes/RptFiltrarClientesPorFecha.rdlc";
            rpt.LocalReport.DataSources.Add(new ReportDataSource("Ds_InformeNueCli", dt.GetData(/*idCli*/ DateTime.Parse(Desde), DateTime.Parse(Hasta)).ToList()));
            rpt.SizeToReportContent = true;
            rpt.ShowPrintButton = true;
            rpt.ShowZoomControl = true;
            ViewBag.rpt = rpt;
            return View();

        }


    }
}