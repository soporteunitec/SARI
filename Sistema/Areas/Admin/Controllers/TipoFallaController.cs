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
    public class TipoFallaController : Controller
    {
        private tipo_falla falla = new tipo_falla();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Crud(int id = 0)
        {
            if (id != 0)
            {
                falla = falla.Obtener(id);
            }

            return View(falla);
        }

        public JsonResult Guardar(tipo_falla model)
        {
            var rm = new ResponseModel();
            if (ModelState.IsValid)
            {
                rm = model.Guardar();
                if (rm.response)
                {
                    rm.href = Url.Content("~/admin/tipofalla");
                }

            }
            return Json(rm);
        }

        public JsonResult Eliminar(int id)
        {
            var rm = new ResponseModel();
            rm = falla.eliminar(id);
            if (rm.response)
            {
                rm.href = Url.Content("self");
            }
            return Json(rm);
        }

        public JsonResult Listar(AnexGRID grid)
        {
            return Json(falla.Listar(grid));
        }
    }
}