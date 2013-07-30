using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace USPeriodico.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) >= 0)
                return Redirect("/Home/IndexSafe");

            return View();
        }

        [Authorize]
        public ActionResult IndexSafe()
        {
            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) < 0)
                return Redirect(FormsAuthentication.LoginUrl);

            return View();
        }

    }
}
