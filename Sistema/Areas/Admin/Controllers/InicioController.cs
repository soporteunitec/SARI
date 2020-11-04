using Rotativa.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Sistema.Areas.Admin.Filters;

namespace Sistema.Areas.Admin.Controllers
{
    [Autenticado]
    public class InicioController : Controller
    {

         
       static private DateTime PrimerDia()
        {
            DateTime referenceDate;
            referenceDate = DateTime.Now.Date;
            DayOfWeek referenceDayOfWeek = referenceDate.DayOfWeek;

            int diffDaysFromMonday = DayOfWeek.Monday - referenceDayOfWeek;
            if (diffDaysFromMonday > 0) { diffDaysFromMonday -= 7; }
            DateTime mondayOfTheWeek = referenceDate.AddDays(diffDaysFromMonday);

            return mondayOfTheWeek;
        }
       static private DateTime UltimoDia()
        {
            DateTime referenceDate;
            referenceDate = DateTime.Now.Date;
            DayOfWeek referenceDayOfWeek = referenceDate.DayOfWeek;

            int diffDaysToSunday = (DayOfWeek.Sunday - referenceDayOfWeek);
            if (diffDaysToSunday < 0) { diffDaysToSunday += 7; }
            DateTime sundayOfTheWeek = referenceDate.AddDays(diffDaysToSunday);

            return sundayOfTheWeek;

            
        }
       static private string Inicio = "";
        static private string Fin = "";



        private reportes reporte = new reportes();
        public ActionResult Index(string inicio = "", string fin = "", bool pdf = false)
        {
            
            if (inicio == "" && fin == "")
            {
                var primer = PrimerDia();
                var segun = UltimoDia();
                ViewBag.ReportesAbiertos = reporte.CantidadReportesAbiertos(PrimerDia(), UltimoDia());
                ViewBag.ReportesCerrados = reporte.CantidadReportesCerrados(PrimerDia(), UltimoDia());
                ViewBag.ReportesAbiertosDpto = reporte.CantidadReportesAbiertosDpto(PrimerDia(), UltimoDia());
                ViewBag.ReportesCerradosDpto = reporte.CantidadReportesCerradosDpto(PrimerDia(), UltimoDia());
                ViewBag.ReportesCerradosPorTecnico = reporte.CantidadReportesCerradosPorTecnico(PrimerDia(), UltimoDia());

                ViewBag.PrestamosAbiertos = reporte.CantidadPrestamosAbiertos(PrimerDia(), UltimoDia());
                ViewBag.PrestamosCerrados = reporte.CantidadPrestamosCerrados(PrimerDia(), UltimoDia());
                ViewBag.inicio = PrimerDia();
                ViewBag.fin = UltimoDia();
                Inicio = PrimerDia().ToShortDateString();
                Fin = UltimoDia().ToShortDateString();
            }
            else
            {
                ViewBag.ReportesAbiertos = reporte.CantidadReportesAbiertos(Convert.ToDateTime(inicio), Convert.ToDateTime(fin));
                ViewBag.ReportesCerrados = reporte.CantidadReportesCerrados(Convert.ToDateTime(inicio), Convert.ToDateTime(fin));
                ViewBag.ReportesAbiertosDpto = reporte.CantidadReportesAbiertosDpto(Convert.ToDateTime(inicio), Convert.ToDateTime(fin));
                ViewBag.ReportesCerradosDpto = reporte.CantidadReportesCerradosDpto(Convert.ToDateTime(inicio), Convert.ToDateTime(fin));
                ViewBag.ReportesCerradosPorTecnico = reporte.CantidadReportesCerradosPorTecnico(Convert.ToDateTime(inicio), Convert.ToDateTime(fin));
                ViewBag.PrestamosAbiertos = reporte.CantidadPrestamosAbiertos(Convert.ToDateTime(inicio), Convert.ToDateTime(fin));
                ViewBag.PrestamosCerrados = reporte.CantidadPrestamosCerrados(Convert.ToDateTime(inicio), Convert.ToDateTime(fin));
                ViewBag.inicio = Convert.ToDateTime(inicio);
                ViewBag.fin = Convert.ToDateTime(fin);
                Inicio = inicio;
                Fin = fin;
            }
            ViewBag.pdf = pdf;
            return View();
        }

        public ActionResult ExportarPDF()
        {
            return new ActionAsPdf("index", new { inicio = Inicio, fin = Fin, pdf = true });
        }

        public ActionResult ReporteGeneral()
        {
            return View();
        }
        
    }
}