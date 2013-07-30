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
        estagioEntities entities2 = new estagioEntities();

        private List<EventoCEPE> eventosCEPE;
        private List<Estagio> estagios;
        //
        // GET: /Home/

        public ActionResult Index()
        {
            int[] eventoID;
            String[] eventosNome;

            eventosCEPE = entities.EventoCEPE.ToList();
            estagios = entities2.Estagio.ToList();

            foreach (EventoCEPE evento in eventosCEPE)
            {
                //eventoID = evento.ID;
                //eventosNome = evento.Nome;
            }

            foreach (Estagio estagio  in estagios)
            {
            
            }

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
