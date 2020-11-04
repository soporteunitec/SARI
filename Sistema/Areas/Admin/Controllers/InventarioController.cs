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
    public class InventarioController : Controller
    {
        private tipoInventario tipoInventario = new tipoInventario();
        private usuarios usuario = new usuarios();
        private inventarios inventario = new inventarios();
        
        public ActionResult Index(bool pdf = false)
        {
            ViewBag.pdf = pdf;
            return View();
        }

             
        public ActionResult Crud(int id = 0)
        {
            ViewBag.tipoInventario = tipoInventario.Listar();
            ViewBag.usuarios = usuario.Listar();
            if (id != 0)
            {
                inventario = inventario.Obtener(id);
            }

            return View(inventario);
        }

        

        public JsonResult Guardar(inventarios model)
        {
            var rm = new ResponseModel();
            if (ModelState.IsValid)
            {
                rm = model.Guardar();
                if (rm.response)
                {
                    rm.href = Url.Content("~/admin/inventario");
                }

            }
            return Json(rm);
        }



        public JsonResult Eliminar(int id)
        {
            var rm = new ResponseModel();
            rm = inventario.eliminar(id);
            if (rm.response)
            {
                rm.href = Url.Content("self");
            }
            return Json(rm);
        }

        public JsonResult Listar(AnexGRID grid)
        {
            return Json(inventario.Listar(grid));
        }
        public ActionResult ExportarPDFinventarios()
        {
            return new ActionAsPdf("index", new { pdf = true });
        }

    }
}