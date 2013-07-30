using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPeriodico.Models;
using System.Data.Entity;
using System.Web.Security;



namespace USPeriodico.Controllers
{   
    public class LoginController : Controller
    {
        private static int ROLE_ADMINISTRADOR = 1;
        private static int ROLE_EMPRESA = 2;
        private static int ROLE_ALUNO = 3;
        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Login()
        {
            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) >= 0)
                return Redirect("/Home/IndexSafe");

            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuarios model)
        {
            if (ModelState.IsValid)
            {
                string email = model.email;
                string password = model.password;
                usperiodicoEntities entities = new usperiodicoEntities();

                bool userValid = entities.Usuarios.Any(Usuarios => Usuarios.email == email && Usuarios.password == password);

                if (userValid)
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                    
                    if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) == 0)
                    {
                        alunoEntities alunoentities = new alunoEntities();
                        int id = entities.Usuarios.First(Usuarios => Usuarios.email == email).Id;
                        if (!alunoentities.Aluno.Any(Aluno => Aluno.ID == id))
                        {
                            return View("CriaCadastro");
                        }
                    }
                    return Redirect("/Home/IndexSafe");
                }
            }
            return Redirect("/Home/");
        }



        [HttpGet]
        public ActionResult Cadastrar()
        {
            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) >= 0)
                return Redirect("/Home/IndexSafe");

            return View();
        }
        [HttpPost]
        public ActionResult Cadastrar(Usuarios model, String confirmaPassword, int tipoUsuario)
        {
            if (ModelState.IsValid)
            {
                usperiodicoEntities entities = new usperiodicoEntities();
                //Papel no sistema
                string criarUsuario = "Home";
                if (tipoUsuario == 2)
                {
                    model.role = ROLE_EMPRESA;
                    criarUsuario = "Login";
                }
                else if (tipoUsuario == 3)
                {
                    model.role = ROLE_ALUNO;
                    criarUsuario = "Login";
                }
                try
                {
                    if (!model.password.Equals(confirmaPassword))
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro = "Senhas diferentes";
                        return View();
                    }
                }
                catch (Exception e) 
                {
                    ViewBag.alert = true;
                    ViewBag.mensagemErro = "Digite uma senha";
                    return View();
                }
                

                //Verifica se já existe o email cadastrado
                if(entities.Usuarios.Any(Usuarios=>Usuarios.email == model.email)){
                    ViewBag.alert = true;
                    ViewBag.mensagemErro = "Usuário já existe!";
                    return View();
                }

                //Verifica formato do e-mail
                if (Utilitarios.TestEmailRegex(model.email))
                {
                    //Verifica se é um aluno e o domínio é da usp
                    if (model.role == ROLE_ALUNO && !Utilitarios.VerificaEmailUSP(model.email))
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro = "e-mail inválido!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.alert = true;
                    ViewBag.mensagemErro = "e-mail inválido!";
                    return View();
                }

                entities.Usuarios.Add(model);
                entities.SaveChanges();
                return Redirect(criarUsuario);
            }
            return Redirect("Home") ;
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return Redirect("/Home/");
        }

    }
}
