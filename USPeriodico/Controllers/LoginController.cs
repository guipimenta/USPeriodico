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
        private static string login;
        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Login()
        {
            if (Utilitarios.VerificaUsuario(ROLE_ADMINISTRADOR, HttpContext.User.Identity.Name) >= 0)
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
                    if (Utilitarios.VerificaUsuario(ROLE_ADMINISTRADOR, email) == 0)
                    {
                        //empresa
                        if (Utilitarios.VerificaUsuario(ROLE_EMPRESA, email) > 0)
                        {
                            alunoEntities alunoentities = new alunoEntities();
                            int id = entities.Usuarios.First(Usuarios => Usuarios.email == email).Id;
                            if (!alunoentities.Aluno.Any(Aluno => Aluno.ID == id))
                            {
                                return Redirect("CriarEmpresa?id=" + email);
                            }
                        }
                        //aluno
                        if (Utilitarios.VerificaUsuario(ROLE_ALUNO, email) > 0)
                        {
                            alunoEntities alunoentities = new alunoEntities();
                            int id = entities.Usuarios.First(Usuarios => Usuarios.email == email).Id;
                            if (!alunoentities.Aluno.Any(Aluno => Aluno.ID == id))
                            {
                                return Redirect("CriarAluno?id=" + email);
                            }
                        }
                    }
                    FormsAuthentication.SetAuthCookie(email, false);
                    return Redirect("/Home/IndexSafe");
                }
            }
            ViewBag.alert = true;
            ViewBag.mensagemErro = "Email ou senha inválidos";
            return View();
        }



        [HttpGet]
        public ActionResult Cadastrar()
        {
            if (Utilitarios.VerificaUsuario(ROLE_ADMINISTRADOR, HttpContext.User.Identity.Name) >= 0)
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
                string criarUsuario = "/Home";
                if (tipoUsuario == ROLE_EMPRESA)
                {
                    model.role = ROLE_EMPRESA;
                    criarUsuario = "CriarEmpresa?id=";
                }
                else if (tipoUsuario == ROLE_ALUNO)
                {
                    model.role = ROLE_ALUNO;
                    criarUsuario = "CriarAluno?id=";
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


                //Verifica se ja existe o email cadastrado
                if (entities.Usuarios.Any(Usuarios => Usuarios.email == model.email))
                {
                    ViewBag.alert = true;
                    ViewBag.mensagemErro = "Usuário já existe!";
                    return View();
                }

                //Verifica formato do e-mail
                if (Utilitarios.TestEmailRegex(model.email))
                {
                    //Verifica se e um aluno e o dominio e da usp
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
                ViewBag.login = model.email;
                return Redirect(criarUsuario + model.email);
            }
            return Redirect("/Home");
        }

        [HttpGet]
        public ActionResult CriarAluno()
        {
            if (Utilitarios.VerificaUsuario(ROLE_ADMINISTRADOR, HttpContext.User.Identity.Name) >= 0)
                return Redirect("/Home/IndexSafe");
            if(!Request.QueryString["id"].Equals(""))
                login = Request.QueryString["id"];
            alunoEntities entities = new alunoEntities();
            List<Curso> todosCursos = entities.Curso.ToList();
            return View(todosCursos);
        }
        [HttpPost]
        public ActionResult CriarAluno(Aluno model)
        {
            alunoEntities novoAluno = new alunoEntities();
            List<Curso> todosCursos = novoAluno.Curso.ToList();
            // Inicializar ViewBag
            ViewBag.alert = false;
            ViewBag.mensagemErro = "";
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Curso == 0)
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro += "Selecione um curso\n";
                    }
                    if (model.Nome.Length == 0)
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro += "Nome vazio\n";
                    }
                    if (model.NUSP.ToString().Length == 0)
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro += "Numero USP vazio\n";
                    }
                    if (model.NUSP.ToString().Length != 7)
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro += "Numero USP inválido\n";
                    }
                    if (model.Telefone.Length == 0)
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro += "Telefone vazio\n";
                    }
                    else if (model.Telefone.Length > 9)
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro += "Telefone muito grande\n";
                    }
                    if (model.Unidade.Length == 0)
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro += "Unidade vazia\n";
                    }
                }
                catch (Exception e)
                {
                    ViewBag.alert = true;
                    ViewBag.mensagemErro = "Complete todos os campos!";
                    return View(todosCursos);
                }
                if (ViewBag.alert == true)
                {
                    return View(todosCursos);
                }

                //Verifica se ja existe o aluno cadastrado
                if (novoAluno.Aluno.Any(Aluno => Aluno.NUSP == model.NUSP))
                {
                    ViewBag.alert = true;
                    ViewBag.mensagemErro = "Usuário com este número USP já existe!";
                    return View(todosCursos);
                }
                usperiodicoEntities aux = new usperiodicoEntities();
                Usuarios usuario = aux.Usuarios.First(Usuario => Usuario.email == login);
                model.ID = usuario.Id;

                model.Curso1 = novoAluno.Curso.First(Curso => Curso.ID == model.Curso);
                novoAluno.Aluno.Add(model);
                novoAluno.SaveChanges();
                FormsAuthentication.SetAuthCookie(login, false);
                return Redirect("/Home/IndexSafe");
            }
            return View(todosCursos);
        }

        [HttpGet]
        public ActionResult CriarEmpresa()
        {
            if (Utilitarios.VerificaUsuario(ROLE_ADMINISTRADOR, HttpContext.User.Identity.Name) >= 0)
                return Redirect("/Home/IndexSafe");
            if (!Request.QueryString["id"].Equals(""))
                login = Request.QueryString["id"];
            return View();
        }
        [HttpPost]
        public ActionResult CriarEmpresa(Empresa model)
        {
            if (ModelState.IsValid)
            {
                empresaEntities novaEmpresa = new empresaEntities();

                try
                {
                    if (model.Descricao.Equals(""))
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro = "Descrição vazia";
                        return View();
                    }
                    if (model.Endereco.Equals(""))
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro = "Endereço vazio";
                        return View();
                    }
                    if (model.Nome.Equals(""))
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro = "Nome vazio";
                        return View();
                    }
                    if (model.Telefone.Equals(""))
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro = "Telefone vazio";
                        return View();
                    }
                    else if (model.Telefone.Length > 9)
                    {
                        ViewBag.alert = true;
                        ViewBag.mensagemErro = "Telefone muito grande";
                        return View();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.alert = true;
                    ViewBag.mensagemErro = "Complete todos os campos!";
                    return View();
                }


                //Verifica se ja existe a empresa cadastrada
                if (novaEmpresa.Empresa.Any(Empresa => Empresa.Nome == model.Nome))
                {
                    ViewBag.alert = true;
                    ViewBag.mensagemErro = "Empresa já existe!";
                    return View();
                }
                usperiodicoEntities aux = new usperiodicoEntities();
                Usuarios usuario = aux.Usuarios.First(Usuario => Usuario.email == login);
                model.ID = usuario.Id;
                novaEmpresa.Empresa.Add(model);
                novaEmpresa.SaveChanges();
                FormsAuthentication.SetAuthCookie(login, false);
                return Redirect("/Home/IndexSafe");
            }
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return Redirect("/Home/");
        }

    }
}
