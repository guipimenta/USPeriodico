using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPeriodico.Models;

namespace USPeriodico.Controllers
{
    public class SincronizadorController : Controller
    {
        //
        // GET: /Sincronizador/EventoCEPE
        [HttpGet]
        public ActionResult EventoCEPE()
        {
            eventoCEPEEntities entities = new eventoCEPEEntities();
            EventoCEPE[] eventos = entities.EventoCEPE.ToArray();
            return Json(eventos, JsonRequestBehavior.AllowGet);
        }

        // GET: /Sincronizador/Estagio
        [HttpGet]
        public ActionResult Estagio()
        {
            estagioEntities entities = new estagioEntities();
            Estagio[] eventos = entities.Estagio.ToArray();
            return Json(eventos, JsonRequestBehavior.AllowGet);
        }

    }
}
