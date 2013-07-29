using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPeriodico.Models;
using System.Web.Security;

namespace USPeriodico.Controllers
{
    public class AdministradorController : Controller
    {
        //
        // GET: /Administrador/
        public ActionResult ListarUsuarios()
        {
            //verifica se é um usuário logado
            if(HttpContext.User.Identity.Name.Equals(""))
                return Redirect(FormsAuthentication.LoginUrl);
            System.Web.Security.Roles.IsUserInRole("Administrador");
            usperiodicoEntities entities = new usperiodicoEntities();
            
            //verifica se o usuário é um administrador
            bool userAdm = entities.Usuarios.Any(Usuarios => Usuarios.email == HttpContext.User.Identity.Name && Usuarios.role == 1);
            if (!userAdm)
                return Redirect("/Home/IndexSafe");

            IQueryable<Usuarios> listaOrdenada = entities.Usuarios.OrderBy(usuario => usuario.role).ThenBy(usuario => usuario.Id);
            ViewBag.usuarios = listaOrdenada;
            return View();
        }

    }
}
