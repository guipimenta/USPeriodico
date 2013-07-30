using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPeriodico.Models;
using System.Web.Security;

namespace USPeriodico.Controllers
{
    public class EventoCEPEController : Controller
    {
        eventoCEPEEntities entities = new eventoCEPEEntities();
        private List<EventoCEPE> todosEventos;
        
        
        //
        // GET: /EventoCEPE/Listar

        [HttpGet]
        public ActionResult Listar()
        {

            //Dois casos: usuario autenticado ou usuario anonimo
            try
                {
                    String name = HttpContext.User.Identity.Name;
                    usperiodicoEntities usuario = new usperiodicoEntities();
                    Usuarios recuperado = usuario.Usuarios.First(Usuario => Usuario.email == name);
                    ViewBag.ID = recuperado.Id;
                    ViewBag.NoID = false;
                }
                catch (Exception e)
                {
                    //Se a autenticacao nao foi feita, entra em modo anonimo
                    ViewBag.NoID = true;
                    ViewBag.ID = null;
                }
                
                
            
            todosEventos = entities.EventoCEPE.ToList();
            return View(todosEventos);
        }

        [HttpGet]
        public ActionResult Detalhar(String id)
        {
            int idint = int.Parse(id);
            EventoCEPE evento = entities.EventoCEPE.Find(idint);
            if (evento != null)
            {
                return View(evento);
            }
            return Redirect("Listar");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Deletar(String id)
        {
            int idint = int.Parse(id);
            EventoCEPE evento = entities.EventoCEPE.Find(idint);
            entities.EventoCEPE.Remove(evento);
            entities.SaveChanges();
            return Redirect("Listar");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Criar()
        {
            //Pega o login do usuario
            String name = HttpContext.User.Identity.Name;
            ViewBag.alert = false;
            usperiodicoEntities usuario = new usperiodicoEntities();
            Usuarios recuperado = usuario.Usuarios.First(Usuario => Usuario.email == name);
            ViewBag.AlunoID = recuperado.Id;


            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Criar(EventoCEPE model)
        {
            try
            {
                model = entities.EventoCEPE.Add(model);
                entities.SaveChanges();
                return Redirect("Editar?Id=" + model.ID);
            }
            catch (Exception e)
            {
                return Redirect("Editar " + e.Message);
            }
            
        }

        [HttpGet]
        [Authorize]
        public ActionResult Editar(string ID)
        {
            if (ID != null)
            {
                int idInt = int.Parse(ID);
                
                EventoCEPE evento = entities.EventoCEPE.Find(idInt);

                if (evento == null)
                    return View("Invalido");
                else
                    return View(evento);
            }

            return Redirect("~/Home/IndexSafe");
                
        }
        [HttpPost]
        [Authorize]
        public ActionResult Editar(EventoCEPE modelModificado)
        {
            //Validacoes
            
            //Persistencia
            try
            {
                if (ModelState.IsValid)
                {
                    //Metodo criado pelo Stored Procedures
                    entities.EventoCEPEUpdate(modelModificado.Nome, modelModificado.Local, modelModificado.Descricao, modelModificado.Esporte, modelModificado.Data, modelModificado.Horario, modelModificado.ID);   
                }
            }
            catch (DbEntityValidationException e)
            {
                ViewBag.alert = true;
                ViewBag.mensagemErro = "Erro: " + e.Message;
                return View();
                
            }
            return Redirect("~/EventoCEPE/Editar?id=" + modelModificado.ID );
            
        }
    }
}
