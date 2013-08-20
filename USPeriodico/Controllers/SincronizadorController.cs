using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPeriodico.Models;

namespace USPeriodico.Controllers
{
    public class SincronizadorController : Controller
    {
        //
        // GET: /Sincronizador/EventoCEPE
        [HttpGet]
        public ActionResult EventoCEPE(string data)
        {
            // Se o parametro data nao puder ser codificado, retornar um Json vazio
            DateTime dataModificacao;
            if (!DateTime.TryParse(data, out dataModificacao))
            {
                JsonEvento eventoCEPE_vazio = new JsonEvento(); // Usando JsonEvento para facilitar Debugging (pela diferenca de campos entre JsonEvento e JsonEventoCEPE)
                return Json(eventoCEPE_vazio, JsonRequestBehavior.AllowGet);
            }

            eventoCEPEEntities eventoCEPE_entities = new eventoCEPEEntities();
            EventoCEPE[] eventosCEPE = eventoCEPE_entities.EventoCEPE.ToArray();

            alunoEntities aluno_entities = new alunoEntities();

            Queue<JsonEventoCEPE> json_eventosCEPE = new Queue<JsonEventoCEPE>();
            foreach (EventoCEPE eventoCEPE in eventosCEPE)
            {
                JsonEventoCEPE json_eventoCEPE = new JsonEventoCEPE();
                json_eventoCEPE.id = eventoCEPE.ID;
                json_eventoCEPE.tipo = "cepe";
                json_eventoCEPE.nome = eventoCEPE.Nome;
                // falta parametro de valido
                json_eventoCEPE.descricao = eventoCEPE.Descricao;
                // falta parametro de image_link
                // falta parametro de data de atualizacao -> extremamente necessario para terminar esse controler devido ao filtro
                json_eventoCEPE.nome_aluno = aluno_entities.Aluno.Find(eventoCEPE.AlunoID).Nome;
                json_eventoCEPE.local = eventoCEPE.Local;
                json_eventoCEPE.data = eventoCEPE.Data;
                json_eventoCEPE.horario = eventoCEPE.Horario;
                json_eventosCEPE.Enqueue(json_eventoCEPE);
            }

            return Json(json_eventosCEPE, JsonRequestBehavior.AllowGet);
        }

        // GET: /Sincronizador/Estagio
        [HttpGet]
        public ActionResult Estagio(string data)
        {
            // Se o parametro data nao puder ser codificado, retornar um Json vazio
            DateTime dataModificacao;
            if (!DateTime.TryParse(data, out dataModificacao))
            {
                JsonEvento estagio_vazio = new JsonEvento();
                return Json(estagio_vazio, JsonRequestBehavior.AllowGet);
            }

            estagioEntities entities = new estagioEntities();
            Estagio[] estagios = entities.Estagio.ToArray();

            Queue<JsonEstagio> json_estagios = new Queue<JsonEstagio>();
            foreach (Estagio estagio in estagios)
            {
                JsonEstagio json_estagio = new JsonEstagio();
                json_estagio.id = estagio.ID;
                json_estagio.tipo = "estagio";
                json_estagio.nome = estagio.BreveDescricao;
                // falta parametro de valido
                json_estagio.descricao = estagio.Descricao;
                // falta parametro de image_link
                // falta parametro de data de atualizacao -> extremamente necessario para terminar esse controler devido ao filtro
                // falta Model da empresa, para pegar o nome dela a partir do ID
                json_estagio.data_inicio = estagio.DataInicio;
                json_estagio.duracao = estagio.Duracao;
                json_estagio.bolsa = Convert.ToDecimal(estagio.Bolsa); // trocar tipo de dado do estagio.Bolsa para Money
                // falta Model da area, para pegar o nome dela a partir do ID
                json_estagios.Enqueue(json_estagio);
            }

            return Json(json_estagios, JsonRequestBehavior.AllowGet);
        }

    }

    public class JsonEvento
    {
        public int id;
        public string tipo;
        public string nome;
        public bool valido;
        public string descricao;
        public string image_link;
        public DateTime data_atualizacao;
    }

    public class JsonEstagio : JsonEvento
    {
        public string nome_empresa;
        public string descricao_empresa;
        public DateTime data_inicio;
        public string duracao;
        public decimal bolsa;
        public string area;
    }

    public class JsonEventoCEPE : JsonEvento
    {
        public string nome_aluno;
        public string local;
        public string esporte;
        public DateTime data;
        public TimeSpan horario;
    }
}
