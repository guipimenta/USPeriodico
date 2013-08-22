using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPeriodico.Models;

namespace USPeriodico.Controllers
{
    public class PalestraController : Controller
    {
        //
        // GET: /Palestra/
        [HttpGet]
        public ActionResult Index(int? mesDesejado = null)
        {
            return Redirect("Listar");
        }

        //
        // GET: /Palestra/Listar
        [HttpGet]
        [Authorize]
        public ActionResult Listar()
        {
            palestraEntities palestraE = new palestraEntities();
            List<palestras> palestraList = palestraE.palestras.ToList();
            return View(palestraList);
        }

        [HttpGet]
        public ActionResult Detalhar(String id)
        {
            int idint = int.Parse(id);
            palestraEntities palestraE = new palestraEntities();
            palestras evento = palestraE.palestras.Find(idint);
            if ( evento != null)
            {
                return View(evento);
            }
            return Redirect("Listar");
        }

    }
}
