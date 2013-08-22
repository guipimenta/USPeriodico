using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPeriodico.Models;


/*
 * Como somente os administradores podem modificar os timers
 * eu deixei todos os metodos como authorize e tambem deixei
 * authorize em TODOS os metodos, menos os referentes ao envio
 * do JSON. Acho que nao ha problemas em relacao a isso, porque
 * todos os eventos enviados no metodo do json sao eventos publicos.
 * 
 * Para o timer funcionar voces precisam ter o projeto pushApp rodando
 * no servidor, caso contrario nao vai atualizar nada.
 */
namespace USPeriodico.Controllers
{
    public class SincronizadorController : Controller
    {

        [HttpGet]
        [Authorize]
        public ActionResult ListaTimer()
        {
            String name = HttpContext.User.Identity.Name;

            if (Utilitarios.VerificaUsuario(1, name) == 1)
            {
                timerEntities timer = new timerEntities();
                List<Timer> todosTimers = timer.Timer.ToList();
                return View(todosTimers);
            }
            else
            {
                String titulo = "Erro ao authenticar usuario";
                String error = "O usuário não possuí permissões suficientes para listar os timers";
                return Redirect("~/Error/Index?titulo="+ titulo + "&mensagem=" + error);
            }

            
        }

        [HttpGet]
        [Authorize]
        public ActionResult Editar(String id)
        {

            String name = HttpContext.User.Identity.Name;

            if (Utilitarios.VerificaUsuario(1, name) == 1)
            {
                timerEntities timerE = new timerEntities();
                Timer timer = timerE.Timer.Find(int.Parse(id));
                return View(timer);
            }
            else
            {
                String titulo = "Erro ao authenticar usuario";
                String error = "O usuário não possuí permissões suficientes para editar timers";
                return Redirect("~/Error/Index?titulo=" + titulo + "&mensagem=" + error);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Editar(Timer timer)
        {

            String name = HttpContext.User.Identity.Name;

            if (Utilitarios.VerificaUsuario(1, name) == 1)
            {
                timerEntities timerE = new timerEntities();
                timerE.TimerUpdate(timer.Id, timer.Horario, timer.Frequencia);
                return View(timer);
            }
            else
            {
                String titulo = "Erro ao authenticar usuario";
                String error = "O usuário não possuí permissões suficientes para editar timers";
                return Redirect("~/Error/Index?titulo=" + titulo + "&mensagem=" + error);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Criar()
        {

            String name = HttpContext.User.Identity.Name;

            if (Utilitarios.VerificaUsuario(1, name) == 1)
            {
                return View();
            }
            else
            {
                String titulo = "Erro ao authenticar usuario";
                String error = "O usuário não possuí permissões suficientes para criar timers";
                return Redirect("~/Error/Index?titulo=" + titulo + "&mensagem=" + error);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Criar(Timer timer)
        {

            String name = HttpContext.User.Identity.Name;

            if (Utilitarios.VerificaUsuario(1, name) == 1)
            {
                timerEntities timerE = new timerEntities();
                decimal timerid= decimal.Parse( ""+ timerE.TimerInsert(timer.Horario, timer.Frequencia).ElementAt(0));
                return Redirect("Editar?id="+timerid);
            }
            else
            {
                String titulo = "Erro ao authenticar usuario";
                String error = "O usuário não possuí permissões suficientes para criar timers";
                return Redirect("~/Error/Index?titulo=" + titulo + "&mensagem=" + error);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Deletar(Timer timer)
        {

            String name = HttpContext.User.Identity.Name;
            
            if (Utilitarios.VerificaUsuario(1, name) == 1)
            {
                timerEntities timerE = new timerEntities();
                timerE.TimerDeletar(timer.Id);
                return Redirect("ListaTimer");
            }
            else
            {
                String titulo = "Erro ao authenticar usuario";
                String error = "O usuário não possuí permissões suficientes para deletar timers";
                return Redirect("~/Error/Index?titulo=" + titulo + "&mensagem=" + error);
            }
        }
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
                json_estagio.bolsa = estagio.Bolsa; // trocar tipo de dado do estagio.Bolsa para Money
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
        public string bolsa;
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
