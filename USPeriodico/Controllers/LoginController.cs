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
                string username = model.username;
                string password = model.password;

                UsuariosEntities entities = new UsuariosEntities();

                bool userValid = entities.Usuarios.Any(Usuarios => Usuarios.username == username);

                if (userValid)
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    return Redirect("/Home/IndexSafe");
                }
            }
            return Redirect("/Home/");
        }


        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return Redirect("/Home/");
        }

    }
}
