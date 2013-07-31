﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using USPeriodico.Models;

namespace USPeriodico.Controllers
{
    public class HomeController : Controller
    {
        
        eventoCEPEEntities entities = new eventoCEPEEntities();
        estagioEntities entities2 = new estagioEntities();

        private List<EventoCEPE> eventosCEPE;
        private List<Estagio> estagios;
        //
        // GET: /Home/

        public ActionResult Index()
        {
            
            eventosCEPE = entities.EventoCEPE.ToList();
            estagios = entities2.Estagio.ToList();
            int[] eventoID = new int[eventosCEPE.Count+ estagios.Count];
            String[] eventosNome = new String[eventosCEPE.Count + estagios.Count];
            int[] eventoTipo = new int[eventosCEPE.Count + estagios.Count];
            DateTime[] eventosData = new DateTime[eventosCEPE.Count + estagios.Count];
            int i=0;
            foreach (EventoCEPE evento in eventosCEPE)
            {
                if (evento.Data >= DateTime.Now)
                {
                    eventoID[i] = evento.ID;
                    eventosNome[i] = evento.Nome;
                    eventosData[i] = evento.Data.Date;
                    eventoTipo[i] = 1;
                }
                i++;
            }

            foreach (Estagio estagio  in estagios)
            {
                if (estagio.DataInicio >= DateTime.Now)
                {
                    eventoID[i] = estagio.ID;
                    eventosNome[i] = estagio.BreveDescricao;
                    eventosData[i] = estagio.DataInicio.Date;
                    eventoTipo[i] = 2;
                }
                i++;
            }

            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) >= 0)
                return Redirect("/Home/IndexSafe");

            ViewBag.eventosID = eventoID;
            ViewBag.eventosNome = eventosNome;
            ViewBag.eventoTipo = eventoTipo;
            ViewBag.eventosData = eventosData;
            ViewBag.eventoSize = i;
            return View();
        }

        [Authorize]
        public ActionResult IndexSafe()
        {
            if (Utilitarios.VerificaUsuario(1, HttpContext.User.Identity.Name) < 0)
                return Redirect(FormsAuthentication.LoginUrl);

            return View();
        }

    }
}
