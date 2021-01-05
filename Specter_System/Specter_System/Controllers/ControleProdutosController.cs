using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Business;
using Specter_System.Models.Servicos.Infaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace Specter_System.Controllers
{
    public class ControleProdutosController : Controller
    {
        private INProduto appCurso = new AppBusinessProduto();

        public ActionResult Index()
        {
            List<Produto> listCursos = new List<Produto>();

            listCursos.Add(
                    new Produto()
                    {
                        CursoId = 0,
                        Nome = "",
                        Modalidade = "",
                        Valor = 0
                    }
                );

            Produtos model = new Produtos()
            {
                cursos = listCursos
            };

            return View(model);

        }

        public ActionResult Create()
        {
            return View();
        }
      
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Create")]
        public ActionResult Create(Produto model)
        {
            var caminhoCurso = this.Caminho_Arquivo(model);

            var fileNamePalestrante = Path.GetFileName(model.Imagem_Palestrante.FileName);
            var caminhoPalestrante = String.Empty;

            try
            {
                if (model.Imagem.ContentLength > 0 && model.Imagem_Palestrante.ContentLength > 0)
                {
                    model.Imagem.SaveAs(caminhoCurso);

                    caminhoPalestrante = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["caminhoPalestrante"]), fileNamePalestrante);
                    model.Imagem_Palestrante.SaveAs(caminhoPalestrante);
                }

                Imagem imagem = new Imagem()
                {
                    NomeCurso = model.Imagem.FileName,
                    CaminhoCurso = caminhoCurso.ToString(),
                    NomePalestrante = model.Imagem_Palestrante.FileName,
                    CaminhoPalestrante = caminhoPalestrante
                };

                Produto curso = new Produto()
                {
                    Nome = model.Nome.ToString().ToUpper(),
                    Data = model.Data,
                    Horario = model.Horario,
                    Carga_Horaria = model.Carga_Horaria,
                    Palestrante = model.Palestrante.ToString().ToUpper(),
                    Sobre = model.Sobre,
                    Informacoes = model.Informacoes,
                    Modalidade = model.Modalidade,
                    Valor = model.Valor,
                    QuantidadeDeVagas = model.QuantidadeDeVagas,
                    QtdVendidos = 0,
                    QtdDisponiveis = model.QuantidadeDeVagas,
                    image = imagem
                };

                curso = this.appCurso.CadastrarCurso(curso);

                if (curso.LabResp.Equals("Curso cadastrado com sucesso"))
                {
                    ViewBag.CadastroCurso = curso.LabResp;

                    return RedirectToAction("../ControleProdutos/Index");
                }
                else
                {
                    Produto modelCurso = new Produto()
                    {
                        LabResp = model.LabResp
                    };

                    return RedirectToAction("create", modelCurso);
                }

            }
            catch (Exception error)
            {
                ViewBag.Message = "Erro ao salvar imagem! " + error.Message;
                return View("Index");
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Pesquisar")]
        public ActionResult Pesquisar(Produtos model)
        {
            Produto curso = new Produto()
            {
                Modalidade = model.curso.Modalidade
            };

            Produtos cursos = new Produtos()
            {
                cursos = this.appCurso.PesquisarCursosPorModalidade(curso)
            };

            return View(cursos);
        }

        public ActionResult Editar(string nome)
        {
            Produto model = new Produto()
            {
                Nome = nome
            };

            model = this.appCurso.PesquisarCurso(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Editar(Produto model)
        {
            model = this.appCurso.AlterarCurso(model);

            return View(model);
        }

        public ActionResult Detalhar(string nome)
        {
            Produto curso = new Produto()
            {
                Nome = nome
            };

            curso = this.appCurso.PesquisarCurso(curso);

            return View(curso);
        }

        public ActionResult Deletar(Produto model)
        {
            model = this.appCurso.PesquisarCurso(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "action", Argument = "Excluir")]
        public ActionResult Excluir(Produto model)
        {
            Produto curso = new Produto()
            {
                Codigo = model.Codigo
            };

            //curso = this.appCurso.ExcluirCurso(model);

            Produto produto = new Produto()
            {
                Nome = "Curso",
                Modalidade = "Presencial"
            };

            this.ExcluirArquivo(produto);

            return RedirectToAction("Index");
        }

        private string Caminho_Arquivo(Produto model)
        {
            var fileNameCurso = Path.GetFileName(model.Imagem.FileName);
            string pathCurso = string.Empty;

            if ("Presencial".Equals(model.Modalidade))
            {
                pathCurso = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["caminhoCursoPresencial"]), fileNameCurso);

            }
            else
            {
                pathCurso = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["caminhoCursoOnline"]), fileNameCurso);
            }

            return pathCurso;
        }

        private bool ExcluirArquivo(Produto model)
        {
            bool resp = false;
           // var fileNameCurso = Path.GetFileName(model.Imagem.FileName);
            string pathCurso = string.Empty;

            try
            {
                if ("Presencial".Equals(model.Modalidade))
                {
                    pathCurso = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["caminhoCursoPresencial"]),model.Nome);
                    System.IO.File.Delete(pathCurso);
                }

                resp = true;
            }
            catch
            {
                resp = false;
            }

            return resp;
        }
    }
}
