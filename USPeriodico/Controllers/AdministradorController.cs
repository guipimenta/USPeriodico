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
        [Authorize]
        public ActionResult ListarUsuarios()
        {
            if(Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) < 0)
                return Redirect(FormsAuthentication.LoginUrl);
            else if(Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) == 0)
                return Redirect("/Home/IndexSafe");

            usperiodicoEntities entities = new usperiodicoEntities();
            IQueryable<Usuarios> listaOrdenada = entities.Usuarios.OrderBy(usuario => usuario.role).ThenBy(usuario => usuario.Id);
            ViewBag.usuarios = listaOrdenada;
            return View();
        }

    }
}
