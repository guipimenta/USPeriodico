﻿using System;
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
        private List<Estagio> todosEstagios;

        //
        // GET: /Estagio/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Listar()
        {
            String name = HttpContext.User.Identity.Name;
            usperiodicoEntities usuario = new usperiodicoEntities();
            Usuarios recuperado = usuario.Usuarios.First(Usuario => Usuario.email == name);
            ViewBag.ID = recuperado.Id;
            todosEstagios = entities.Estagio.ToList();
            return View(todosEstagios);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Detalhar(Int32 id)
        {
            Estagio estagio = entities.Estagio.Find(id);
            if (estagio != null)
            {
                return View(estagio);
            }
            return Redirect("Listar");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeletarEstagio(Int32 id)
        {
            Estagio estagio = entities.Estagio.Find(id);
            entities.Estagio.Remove(estagio);
            entities.SaveChanges();
            return Redirect("Listar");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Criar()
        {
            //Pega o login do usuario
            String name = HttpContext.User.Identity.Name;
            ViewBag.alert = false;
            usperiodicoEntities usuario = new usperiodicoEntities();
            Usuarios recuperado = usuario.Usuarios.First(Usuario => Usuario.email == name);
            ViewBag.EmpresaID = recuperado.Id;

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Criar(Estagio model)
        {
            try
            {
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
            if (id != null)
            {

                Estagio estagio = entities.Estagio.Find(id);

                if (estagio == null)
                    return View("Invalido");
                else
                    return View(estagio);
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
                ViewBag.mensagemErro = "Erro ao inserir no Banco de Dados";

            }
            return Redirect("~/EventoCEPE/Editar?id=" + modelModificado.ID);

        }

    }
}