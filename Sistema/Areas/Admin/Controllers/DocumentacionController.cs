using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.IO;
using Sistema.Areas.Admin.Filters;
using Rotativa.MVC;

namespace Sistema.Areas.Admin.Controllers
{
    [Autenticado]
    public class DocumentacionController : Controller
    {
        private documentacion Documentacion = new documentacion();
        private AdjuntosDocumentacion adjunto = new AdjuntosDocumentacion();
        public ActionResult Index(bool pdf = false)
        {
            ViewBag.pdf = pdf;
            return View();
        }

        public ActionResult Crud(int id = 0)
        {
            if (id != 0)
            {
                Documentacion = Documentacion.Obtener(id);
            }

            return View(Documentacion);
        }

        public JsonResult Guardar(documentacion model)
        {
            var rm = new ResponseModel();
            if (ModelState.IsValid)
            {
                rm = model.Guardar();
                if (rm.response)
                {
                    rm.href = Url.Content("~/admin/documentacion");
                }

            }
            return Json(rm);
        }

        public JsonResult Eliminar(int id)
        {
            var rm = new ResponseModel();
            rm = Documentacion.eliminar(id);
            if (rm.response)
            {
                rm.href = Url.Content("self");
            }
            return Json(rm);
        }

        public ActionResult EliminarAdjunto(int id, int id_documento)
        {
        //    Documentacion = Documentacion.Listar
            adjunto = adjunto.Obtener(id);
            System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Uploads/" + adjunto.archivo));

            adjunto.eliminarAdjunto(id);
            return Redirect("~/admin/documentacion/crud/" + id_documento);
            
        }

        public JsonResult Listar(AnexGRID grid)
        {
            return Json(Documentacion.Listar(grid));
        }

        public JsonResult GuardarAdjunto(AdjuntosDocumentacion model, HttpPostedFileBase Archivo)
        {
            var rm = new ResponseModel();

            if(Archivo != null)
            {
                // Nombre del archivo, es decir, lo renombramos para que no se repita nunca
                //  string archivo = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(Archivo.FileName);
                string archivo = Path.GetFileNameWithoutExtension(Archivo.FileName) +"-"+ DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(Archivo.FileName);

                // La ruta donde lo vamos guardar
                Archivo.SaveAs(Server.MapPath("~/Areas/Admin/Uploads/" + archivo));

                // Establecemos en nuestro modelo el nombre del archivo
                model.archivo = archivo;

                rm = model.Guardar();
                if (rm.response)
                {
                    rm.function = "CargarAdjuntos()";
                }
            }

            rm.SetResponse(false, "Debe adjuntar un archivo");
            return Json(rm);
        }

        public PartialViewResult Adjuntos(int id)
        {
            ViewBag.adjuntos = adjunto.Listar(id);
            ViewBag.idDocumento = id;
            return PartialView();
        }

        public ActionResult ExportarPDFdocumentos()
        {
            return new ActionAsPdf("index", new { pdf = true });
        }

    }
}