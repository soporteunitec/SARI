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
    public class DepartamentosController : Controller
    {
        private dptos departamentos = new dptos();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Crud(int id = 0)
        {
            if(id != 0)
            {
                departamentos = departamentos.Obtener(id);
            }
           
            return View(departamentos);
        }

        public JsonResult Guardar(dptos model)
        {
            var rm = new ResponseModel();
            if (ModelState.IsValid)
            {
                rm = model.Guardar();
                if(rm.response)
                {
                    rm.href = Url.Content("~/admin/departamentos");
                }
               
            }
            return Json(rm);
        }

        public JsonResult Eliminar(int id)
        {
            var rm = new ResponseModel();
            rm = departamentos.eliminar(id);
            if (rm.response)
            {
                rm.href = Url.Content("self");
            }
            return Json(rm);
        }

        public JsonResult Listar(AnexGRID grid)
        {
            return Json(departamentos.Listar(grid));
        }
    }
}