using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Business;
using Specter_System.Models.Servicos.Infaces;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Specter_System.Controllers
{
    public class CursosPresenciaisController : Controller
    {
        private INProduto appProduto = new AppBusinessProduto();

        public ActionResult Index()
        {
            Produto curso = new Produto()
            {
                Modalidade = "Presencial"
            };

            List<Produto> cursos = appProduto.ListarCursos(curso);

            return View(cursos);
        }

        public ActionResult Sobre(string nome)
        {
            Produto curso = new Produto()
            {
                Nome = nome
            };

            curso = this.appProduto.PesquisarCurso(curso);

            return View(curso);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument="ExibirCurriculo")]
        public ActionResult ExibirCurriculo(Produto model)
        {
            string link = this.appProduto.Pesquisar_Link(model);

            return Redirect(link);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "RealizarIncricao")]
        public ActionResult RealizarInscricao(Produto model)
        {
            model = this.appProduto.PesquisarCurso(model);

            var produto = new Produto()
            {
                Codigo = model.Codigo,
                Nome = model.Nome,
                Data = model.Data,
                Horario = model.Horario,
                Valor = model.Valor,
                Modalidade = model.Modalidade,
                Palestrante = model.Palestrante
            };

            this.Session["sessionProduto"] = produto;
            Session["sessionValorProduto"] = model.Valor; //Criado a Session para calcular o valor do Produto para inserir no BANCO

            return RedirectToAction("../Home/Login");
        }
    }
}
