using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Business;
using Specter_System.Models.Servicos.Infaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace Specter_System.Controllers
{
    public class CursosOnlineController : Controller
    {
        private INProduto appProduto = new AppBusinessProduto();
        public ViewResult Index()
        {
            Produto curso = new Produto()
            {
                Modalidade = "Online"
            };

            List<Produto> cursos = appProduto.ListarCursos(curso);

            return View(cursos);
        }

        public ActionResult Sobre(string nome)
        {
            Produto curso = new Produto()
            {
                Nome = "DESENVOLVIMENTO WEB"
            };

            curso = this.appProduto.Pesquisar_Sobre_Produto_Online(curso);

            return View(curso);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CarrinhoCompra")]
        public ActionResult CarrinhoCompra(Produto model)
        {
            model = this.appProduto.Pesquisar_Sobre_Produto_Online(model);

            this.Session["sessionProduto"] = model;
            Session["sessionValorProduto"] = model.Valor;

            return View(model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "FinalizarCompra")]
        public ActionResult FinalizarCompra(Produto model)
        {
            return RedirectToAction("../Home/Login");
        }

    }
}
