using Model;
using Rotativa.MVC;
using Sistema.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Areas.Admin.Controllers
{
    [Autenticado]
    public class PrestamosController : Controller
    {
        private prestamos prestamo = new prestamos();
        private usuarios usuario = new usuarios();
        
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
            
            ViewBag.usuarios = usuario.Listar();
            if (id != 0)
            {
                prestamo = prestamo.Obtener(id);
            }

            return View(prestamo);
        }

        public ActionResult Cerrar(int id = 0)
        {
            if (id != 0)
            {
                prestamo = prestamo.Obtener(id);
            }
            return View(prestamo);
        }

        public ActionResult PrestamoCerradoDescripcion(int id)
        {
            ViewBag.usuarios = usuario.Listar();
            prestamo = prestamo.Obtener(id);
            return View(prestamo);
        }

        public JsonResult Guardar(prestamos model)
        {
            var rm = new ResponseModel();
            if (ModelState.IsValid)
            {
                rm = model.Guardar();
                if (rm.response)
                {
                    rm.href = Url.Content("~/admin/prestamos");
                }

            }
            return Json(rm);
        }

        public JsonResult CerrarCaso(prestamos model)
        {
            var rm = new ResponseModel();
            if (ModelState.IsValid)
            {
                rm = model.CerrarPrestamo();
                if (rm.response)
                {
                    rm.href = Url.Content("~/admin/prestamos");
                }

            }
            return Json(rm);
        }

        public JsonResult Eliminar(int id)
        {
            var rm = new ResponseModel();
            rm = prestamo.eliminar(id);
            if (rm.response)
            {
                rm.href = Url.Content("self");
            }
            return Json(rm);
        }

        public JsonResult Listar(AnexGRID grid)
        {
            return Json(prestamo.Listar(grid));
        }


        public JsonResult ListarCerrados(AnexGRID grid)
        {
            return Json(prestamo.ListarCerrados(grid));
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