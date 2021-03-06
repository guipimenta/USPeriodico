﻿using System;
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

        eventoCEPEEntities entities = new eventoCEPEEntities();
        estagioEntities entities2 = new estagioEntities();

        private List<EventoCEPE> eventosCEPE;
        private List<Estagio> estagios;

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

        [Authorize]
        [HttpGet]
        public ActionResult CriarTimer()
        {
            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) < 0)
                return Redirect(FormsAuthentication.LoginUrl);
            else if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) == 0)
                return Redirect("/Home/IndexSafe");

            return View();
        }

        [Authorize]
        public ActionResult Historico()
        {
            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) < 0)
                return Redirect(FormsAuthentication.LoginUrl);
            else if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) == 0)
                return Redirect("/Home/IndexSafe");


            palestraEntities palestraE = new palestraEntities();
            List<palestras> palestrasList = palestraE.palestras.ToList();
            eventosCEPE = entities.EventoCEPE.ToList();
            estagios = entities2.Estagio.ToList();
            int[] eventoID = new int[eventosCEPE.Count + estagios.Count + palestrasList.Count];
            int[] eventoTipo = new int[eventosCEPE.Count + estagios.Count + palestrasList.Count];
            String[] eventosNome = new String[eventosCEPE.Count + estagios.Count + palestrasList.Count];
            String[] eventosDataString = new String[eventosCEPE.Count + estagios.Count + palestrasList.Count];
            DateTime[] eventosData = new DateTime[eventosCEPE.Count + estagios.Count + palestrasList.Count];
            int i = 0;
            foreach (EventoCEPE evento in eventosCEPE)
            {
                eventoID[i] = evento.ID;
                eventosNome[i] = evento.Nome;
                eventosData[i] = evento.Data;
                eventosDataString[i] = "" + evento.Data.Day + "/" + evento.Data.Month + "/" + evento.Data.Year;
                eventoTipo[i] = 1;
                i++;
            }

            foreach (Estagio estagio in estagios)
            {
                eventoID[i] = estagio.ID;
                eventosNome[i] = estagio.BreveDescricao;
                eventosData[i] = estagio.DataInicio;
                eventosDataString[i] = "" + estagio.DataInicio.Day + "/" + estagio.DataInicio.Month + "/" + estagio.DataInicio.Year;
                eventoTipo[i] = 2;
                i++;
            }


            foreach (palestras palestraS in palestrasList)
            {
                    eventoID[i] = palestraS.ID;
                    eventosNome[i] = palestraS.nome;
                    eventosData[i] = (DateTime)palestraS.dataInicio;
                    eventosDataString[i] = "" + eventosData[i].Day + "/" + eventosData[i].Month;
                    eventoTipo[i] = 3;
                i++;
            }

            ViewBag.eventosID = eventoID;
            ViewBag.eventosNome = eventosNome;
            ViewBag.eventoTipo = eventoTipo;
            ViewBag.eventosData = eventosData;
            ViewBag.eventosDataString = eventosDataString;
            ViewBag.eventoSize = i;
            return View();

        }

        [Authorize]
        public ActionResult Timers()
        {
            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) < 0)
                return Redirect(FormsAuthentication.LoginUrl);
            else if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) == 0)
                return Redirect("/Home/IndexSafe");


            return View();
        }


        [Authorize]
        public ActionResult Adicionar()
        {
            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) < 0)
                return Redirect(FormsAuthentication.LoginUrl);
            else if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) == 0)
                return Redirect("/Home/IndexSafe");

            return View();
        }
    }
}
