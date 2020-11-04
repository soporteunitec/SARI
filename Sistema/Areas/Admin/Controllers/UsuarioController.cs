using Helper;
using Model;
using Sistema.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Sistema.Areas.Admin.Controllers
{
    [Autenticado]
    public class UsuarioController : Controller
    {
        private reportes reporte = new reportes();
        private usuarios usuario = new usuarios();
        // GET: Admin/Usuario
        public ActionResult Index()
        {
            return View(usuario.Obtener(SessionHelper.GetUser()));
        }

        public PartialViewResult CambioClave()
        {
            return PartialView(usuario);
        }

        public ActionResult Logros()
        {
            ViewBag.ReportesCerrados = reporte.CantidadReportesCerradosTecnicoTotal(SessionHelper.GetUser());
            return View();
        }

        public JsonResult Guardar(usuarios model)
        {
            var rm = new ResponseModel();

            ModelState.Remove("clave"); 
            if (ModelState.IsValid)
            {
                rm = model.Guardar();
            }
            return Json(rm);
        }


        public JsonResult CambiarClave(usuarios model)
        {
            var rm = new ResponseModel();

            ModelState.Remove("nombre");
            ModelState.Remove("apellido");
            ModelState.Remove("correo");
            ModelState.Remove("fecha_n");
            if (ModelState.IsValid)
            {
                rm = model.CambiarClave();
            }
            if (rm.response)
            {
                rm.href = Url.Content("self");
            }
            return Json(rm);
        }

    }
}