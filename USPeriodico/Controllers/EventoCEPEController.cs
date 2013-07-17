using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPeriodico.Models;

namespace USPeriodico.Controllers
{
    public class EventoCEPEController : Controller
    {
        eventoCEPEEntities entities = new eventoCEPEEntities();
        //
        // GET: /EventoCEPE/Listar

        [HttpGet]
        [Authorize]
        public ActionResult Listar()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Editar(string id)
        {
            if (id != null)
            {
                int idInt = int.Parse(id);

                EventoCEPE evento = entities.EventoCEPE.Find(idInt);
               
                    return View(evento);
            }

            return Redirect("~/Home/IndexSafe");
                
        }
        [HttpPost]
        [Authorize]
        public ActionResult Editar(EventoCEPE model)
        {
            //Validacoes
            
            //Persistencia
            try
            {
                if (ModelState.IsValid)
                {
                    entities.EventoCEPE.Attach(model);
                    entities.Entry(model).State = System.Data.EntityState.Modified;
                    entities.SaveChanges();
                    return Redirect("Editar?ID=1");
                }
            }
            catch (DbEntityValidationException e)
            {
                ViewBag.alert = true;
                ViewBag.mensagemErro = "Erro ao inserir no Banco de Dados";
                
            }
            return Redirect("Editar");
            
        }
    }
}
