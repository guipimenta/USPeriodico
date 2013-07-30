using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using USPeriodico.Models;

namespace USPeriodico.Controllers
{
    public class HomeController : Controller
    {
        eventoCEPEEntities entities = new eventoCEPEEntities();
        private List<EventoCEPE> todosEventos;
        //
        // GET: /Home/

        public ActionResult Index()
        {


            todosEventos = entities.EventoCEPE.ToList();

            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) >= 0)
                return Redirect("/Home/IndexSafe");

            return View(todosEventos);
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
