using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Sistema.Areas.Admin.Filters;
using Rotativa.MVC;

namespace Sistema.Areas.Admin.Controllers
{
    [Autenticado]
    public class ReportesController : Controller
    {
        private reportes reporte = new reportes();
        private dptos departamentos = new dptos();
        private usuarios usuario = new usuarios();
        private tipo_falla tipoFalla = new tipo_falla();
        public ActionResult Index(bool pdf = false)
        {
            ViewBag.pdf = pdf;
            return View();
        }

        public ActionResult Cerrados(bool pdf = false)
        {
            ViewBag.pdf = pdf;
            return View();
        }

        public ActionResult Crud(int id = 0)
        {
            ViewBag.departamentos = departamentos.Listar();
            ViewBag.usuarios = usuario.Listar();
            if (id != 0)
            {
                reporte = reporte.Obtener(id);
            }

            return View(reporte);
        }

        public ActionResult Cerrar(int id = 0)
        {
            ViewBag.tipoFalla = tipoFalla.Listar();
            if (id != 0)
            {
                reporte = reporte.Obtener(id);
            }
            return View(reporte);
        }

        public ActionResult CasoCerradoDescripcion(int id)
        {
            ViewBag.tipoFalla = tipoFalla.Listar();
            ViewBag.departamentos = departamentos.Listar();
            ViewBag.usuarios = usuario.Listar();
            reporte = reporte.Obtener(id);
            return View(reporte);
        }

        public JsonResult Guardar(reportes model)
        {
             var rm = new ResponseModel();
            if (ModelState.IsValid)
            {
                rm = model.Guardar();
                if (rm.response)
                {
                    rm.href = Url.Content("~/admin/reportes");
                }

            }
            return Json(rm);
        }

        public JsonResult CerrarCaso(reportes model)
        {
            var rm = new ResponseModel();
            if (ModelState.IsValid)
            {
                rm = model.CerrarCaso();
                if (rm.response)
                {
                    rm.href = Url.Content("~/admin/reportes");
                }

            }
            return Json(rm);
        }

        public JsonResult Eliminar(int id)
        {
            var rm = new ResponseModel();
            rm = reporte.eliminar(id);
            if (rm.response)
            {
                rm.href = Url.Content("self");
            }
            return Json(rm);
        }

        public JsonResult Listar(AnexGRID grid)
        {
            return Json(reporte.Listar(grid));
        }


        public JsonResult ListarCerrados(AnexGRID grid)
        {
            return Json(reporte.ListarCerrados(grid));
        }

        public ActionResult ExportarPDFAbiertos()
        {
            return new ActionAsPdf("index", new { pdf = true });
        }
        public ActionResult ExportarPDFCerrados()
        {
            return new ActionAsPdf("Cerrados", new { pdf = true });
        }

    }
}