﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Sistema.Areas.Admin.Filters;

namespace Sistema.Areas.Admin.Controllers
{
    [Autenticado]
    public class TipoInventarioController : Controller
    {
        private tipoInventario tipoInventario = new tipoInventario();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Crud(int id = 0)
        {
            if (id != 0)
            {
                tipoInventario = tipoInventario.Obtener(id);
            }

            return View(tipoInventario);
        }

        public JsonResult Guardar(tipoInventario model)
        {
            var rm = new ResponseModel();
            if (ModelState.IsValid)
            {
                rm = model.Guardar();
                if (rm.response)
                {
                    rm.href = Url.Content("~/admin/TipoInventario");
                }

            }
            return Json(rm);
        }

        public JsonResult Eliminar(int id)
        {
            var rm = new ResponseModel();
            rm = tipoInventario.eliminar(id);
            if (rm.response)
            {
                rm.href = Url.Content("self");
            }
            return Json(rm);
        }

        public JsonResult Listar(AnexGRID grid)
        {
            return Json(tipoInventario.Listar(grid));
        }
    }
}