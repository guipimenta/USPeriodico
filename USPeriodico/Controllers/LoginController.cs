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
        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Login()
        {
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
                    return Redirect("/Home/IndexSafe");
                }
            }
            return Redirect("/Home/");
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Cadastrar(Usuarios model)
        {
            if (ModelState.IsValid)
            {
                usperiodicoEntities entities = new usperiodicoEntities();
                model.role = 1;
               /* entities.Usuarios.Any(Usuarios=>Usuarios.email == model.email){

                }*/
                entities.Usuarios.Add(model);
                entities.SaveChanges();
                return Redirect("Login");
               
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
