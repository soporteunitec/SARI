using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Helper;
using Sistema.Areas.Admin.Filters;

namespace Sistema.Areas.Admin.Controllers
{
    
    public class LoginController : Controller
    {
        private usuarios usuario = new usuarios();

        [NoLogin]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Acceder(string Email, string Password)
        {
            var rm = usuario.Acceder(Email, Password);
            if (rm.response)
            {
                rm.href = Url.Content("~/admin/inicio");
            }
            return Json(rm);
        }

        public ActionResult logout()
        {
            SessionHelper.DestroyUserSession();
            return Redirect("~/admin/login");
        }
    }
}