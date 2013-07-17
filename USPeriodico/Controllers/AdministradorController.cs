using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPeriodico.Models;

namespace USPeriodico.Controllers
{
    public class AdministradorController : Controller
    {
        //
        // GET: /Administrador/
        public ActionResult ListarUsuarios()
        {
            usperiodicoEntities entities = new usperiodicoEntities();
            IQueryable<Usuarios> listaOrdenada = entities.Usuarios.OrderBy(usuario => usuario.role).ThenBy(usuario => usuario.Id);
            ViewBag.usuarios = listaOrdenada;
            return View();
        }

    }
}
