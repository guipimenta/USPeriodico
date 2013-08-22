using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USPeriodico.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        [HttpGet]
        public ActionResult Index(string titulo, string mensagem)
        {
            ViewBag.titulo = titulo;
            ViewBag.error = mensagem;
            return View();
        }

    }
}
