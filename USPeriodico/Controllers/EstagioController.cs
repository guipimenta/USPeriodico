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
    public class EstagioController : Controller
    {

        estagioEntities entities = new estagioEntities();

        //
        // GET: /Estagio/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Listar()
        {
            try
            {
                String name = HttpContext.User.Identity.Name;
                usperiodicoEntities usuario = new usperiodicoEntities();
                Usuarios recuperado = usuario.Usuarios.First(Usuario => Usuario.email == name);
                //Verifica se o usuario possui role de empresa
                if (recuperado.role == 2 || recuperado.role == 1)
                {
                    ViewBag.ID = recuperado.Id;
                    ViewBag.NoID = false;
                }
                else
                {
                    ViewBag.NoID = true;
                    ViewBag.ID = null;
                }
            }
            catch (Exception e)
            {
                //Se a autenticacao nao foi feita, entra em modo anonimo
                ViewBag.NoID = true;
                ViewBag.ID = null;
            }

            List<Estagio> estagios = new List<Estagio>();
            foreach(Estagio estagio in entities.Estagio.ToList())
            {
                if(estagio.Valido == true)
                    estagios.Add(estagio);
            }
            return View(estagios);
        }

        [HttpGet]
        public ActionResult Detalhar(Int32 id)
        {
            Estagio estagio = entities.Estagio.Find(id);
            if (estagio != null)
            {
                return View(estagio);
            }
            return Redirect("Listar");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Deletar(Int32 id)
        {
            if (Utilitarios.VerificaUsuario(2, HttpContext.User.Identity.Name) < 0)
                return Redirect(FormsAuthentication.LoginUrl);
            else if (Utilitarios.VerificaUsuario(2, HttpContext.User.Identity.Name) == 0)
                return Redirect("/Home/IndexSafe");

            Estagio estagio = entities.Estagio.Find(id);
            usperiodicoEntities empresa = new usperiodicoEntities();
            Usuarios dono = empresa.Usuarios.Find(HttpContext.User.Identity.Name);
            if (Utilitarios.VerificaUsuario(1, dono.email) > 1)
            {
                entities.Estagio.Remove(estagio);
                entities.SaveChanges();
            }
            else if (Utilitarios.VerificaUsuario(2, dono.email) > 1)
            {
                if (estagio.EmpresaID == dono.Id)
                {
                    entities.Estagio.Remove(estagio);
                    entities.SaveChanges();
                }
            }
            return Redirect("Listar");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Criar()
        {
            //Pega o login do usuario
            String name = HttpContext.User.Identity.Name;

            if (Utilitarios.VerificaUsuario(2, name) < 0)
                return Redirect(FormsAuthentication.LoginUrl);
            else if (Utilitarios.VerificaUsuario(2, name) == 0)
                return Redirect("/Home/IndexSafe");

            ViewBag.alert = false;
            usperiodicoEntities usuario = new usperiodicoEntities();
            Usuarios recuperado = usuario.Usuarios.First(Usuario => Usuario.email == name);
            
            if (recuperado.role == 2 || recuperado.role == 1)
            {
                ViewBag.EmpresaID = recuperado.Id;
            }
            else
            {
                return Redirect("~/SafeIndex");
            }
            

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Criar(Estagio model)
        {
            try
            {
                model.Valido = true;
                model = entities.Estagio.Add(model);
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
        public ActionResult Editar(Int32 id)
        {
            if (Utilitarios.VerificaUsuario(2, HttpContext.User.Identity.Name) < 0)
                return Redirect(FormsAuthentication.LoginUrl);
            else if (Utilitarios.VerificaUsuario(2, HttpContext.User.Identity.Name) == 0)
                return Redirect("/Home/IndexSafe");

            if (id > 0)
            {

                Estagio estagio = entities.Estagio.Find(id);

                if (estagio == null)
                    return View("Invalido");
                else
                {
                    String name = HttpContext.User.Identity.Name;
                    usperiodicoEntities usuario = new usperiodicoEntities();
                    Usuarios recuperado = usuario.Usuarios.First(Usuario => Usuario.email == name);
                    if (Utilitarios.VerificaUsuario(1, recuperado.email) > 1)
                    {
                        ViewBag.ID = id;
                        return View(estagio);
                    }
                    else if (Utilitarios.VerificaUsuario(2, recuperado.email) > 1)
                    {
                        if (recuperado.Id == estagio.EmpresaID)
                        {
                            ViewBag.ID = id;
                            return View(estagio);
                        }
                        else
                        {
                            return Redirect("Listar");
                        }
                    }
               
                }
                    
            }

            return Redirect("~/Home/IndexSafe");

        }
        [HttpPost]
        [Authorize]
        public ActionResult Editar(Estagio modelModificado)
        {
            //Validacoes

            //Persistencia
            try
            {
                if (ModelState.IsValid)
                {
                    //Metodo criado pelo Stored Procedures
                    entities.EstagioUpdate(modelModificado.EmpresaID, modelModificado.Descricao, modelModificado.BreveDescricao, modelModificado.DataInicio, modelModificado.Duracao, modelModificado.Bolsa,modelModificado.Area, modelModificado.ID);
                }
            }
            catch (DbEntityValidationException e)
            {
                ViewBag.alert = true;
                ViewBag.mensagemErro = "Erro ao inserir no Banco de Dados " + e.Message;

            }
            return Redirect("~/Estagio/Editar?id=" + modelModificado.ID);

        }

    }
}
