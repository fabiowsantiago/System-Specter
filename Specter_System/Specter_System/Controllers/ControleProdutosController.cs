using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Business;
using Specter_System.Models.Servicos.Infaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Specter_System.Controllers
{
    public class ControleProdutosController : Controller
    {
        private INProduto appCurso = new AppBusinessProduto();
        private INArquivo appArquivo = new AppBusinessArquivos();
        private INModulo appModulo = new AppBusinessModulo();

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

            Produto produto = new Produto();

            Produtos model = new Produtos()
            {
                cursos = listCursos,
                curso = produto
            };

            return View(model);

        }

        public ActionResult CreateProdutoPresencial()
        {
            Produto produto = new Produto();

            return View(produto);
        }
      
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Create")]
        public ActionResult Create(Produto model)
        {
            var caminhoCurso = this.Caminho_Arquivo(model,"imagem");

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

        public ActionResult CreateProdutoOnline()
        {
            Produto produto = new Produto();

            return View(produto);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CreateProdutoOnline")]
        public ActionResult CreateProdutoOnline(Produto model/*string nome, string cargaHoraria,decimal valor, HttpPostedFileBase imagem*/)
        {
           
            string respImagem = this.SalvarImagem(model);
            string respVideo = this.Salvar_Videos(model);

            List<Video> listVideos = new List<Video>();

            foreach(var item in model.Videos)
            {
                listVideos.Add(
                    new Video
                    {
                        Nome = item.FileName,
                        Caminho = "\\Models\\videos\\" + model.Nome +"\\"+item.FileName
                    }
                );
            }

            Imagem photo = new Imagem()
            {
                NomeCurso = model.Imagem.FileName,
                CaminhoCurso = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["caminhoCursoOnline"]), model.Imagem.FileName)
            };

            if ("salvo".Equals(respImagem) && "salvo".Equals(respVideo))
            {
                Produto produto = new Produto()
                {
                    Nome = model.Nome,
                    Carga_Horaria = model.Carga_Horaria,
                    Valor = model.Valor,
                    image = photo,
                    ListVideos = listVideos,
                    Modalidade = "Online"
                };

                ViewBag.Message = this.appCurso.CadastrarCursoOnline(produto);
            }

            return View();
        }

        [HttpPost]
        public ActionResult SalvarModulos(List<string> modulos, string nomeCurso)
        {
            List<Modulo> listaModulos = new List<Modulo>();

            foreach(var item in modulos)
            {
                listaModulos.Add
                    (
                        new Modulo()
                        {
                            Nome = item
                        }
                    );
            }

            Produto model = new Produto()
            {
                Modulos = listaModulos,
                Nome = nomeCurso
            };

            ViewBag.Message = this.appModulo.SalvarModulos(model);

            return RedirectToAction("CreateProdutoOnline");
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

        private string Caminho_Arquivo(Produto model, string tipoArquivo)
        {
            
            string pathCurso = string.Empty;

            if ("video".Equals(tipoArquivo))
            {
                var fileName = Path.GetFileName(model.Nome);
                pathCurso = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["caminhoVideos"]), fileName);
            }
            else
            {
                var fileNameCurso = Path.GetFileName(model.Imagem.FileName);

                if ("Presencial".Equals(model.Modalidade))
                {
                    pathCurso = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["caminhoCursoPresencial"]), fileNameCurso);

                }
                else
                {
                    pathCurso = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["caminhoCursoOnline"]), fileNameCurso);
                }
            }

            return pathCurso;
        }

        public string SalvarImagem(Produto model)
        {
            string resp = string.Empty;
            string path = string.Empty;

            var fileNameCurso = Path.GetFileName(model.Imagem.FileName);

            if(model.Imagem.ContentLength > 0)
            {
                if ("Presencial".Equals(model.Modalidade))
                {
                    path = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["caminhoCursoPresencial"]), fileNameCurso);
                }
                else
                {
                    path = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["caminhoCursoOnline"]), fileNameCurso);
                }

                try
                {
                    model.Imagem.SaveAs(path);

                    resp = "salvo";
                }
                catch (Exception error)
                {
                    throw new Exception($"Error! {error.Message}");
                }
            }

            return resp;
        }

        private string Salvar_Videos(Produto model)
        {
            string resp = string.Empty;
            string pathVideo = string.Empty;
            string pathVideoWebConfig = string.Empty;
            List<Video> listVideos = new List<Video>();
            string pathImagem = string.Empty;

            try
            {
                string arquivo = model.Nome;

                if (Directory.Exists(pathVideo))
                {
                    resp = "Diretorio ja existe";
                }
                else
                {
                    //Pega a pasta RAIZ e combina com o nome do PRODUTO para criar uma SUBPASTA
                    pathVideoWebConfig = Server.MapPath(ConfigurationManager.AppSettings["caminhoVideos"]) + "\\" + model.Nome;
                    pathVideo = model.Nome;
                    DirectoryInfo di = Directory.CreateDirectory(pathVideoWebConfig);

                        foreach (var item in model.Videos)
                        {
                            if (item.ContentLength > 0)
                            {
                                arquivo = Path.Combine(pathVideoWebConfig + "\\", item.FileName);

                                item.SaveAs(arquivo);

                                listVideos.Add
                                    (
                                        new Video()
                                        {
                                            Nome = item.FileName,
                                            Caminho = pathVideo + "\\" + item.FileName
                                        }
                                    );
                            }
                
                        resp = "salvo";
                    }   
                }
            }
            catch (Exception error)
            {
                throw new Exception($"Erro ao criar diretorio! {error.Message}");
            }
            return resp;
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

        [HttpPost]
        public ActionResult Teste(string nome,string cargaHoraria,decimal valor, List<string> modulos)
        {
            List<Modulo> listaModulos = new List<Modulo>();

            for(int i=0;i<modulos.Count; i++)
            {
                listaModulos.Add
               (
                    new Modulo()
                    {
                        Nome = modulos[i].ToString()
                    }     
               );   
            };

            Produto model = new Produto()
            {
                Nome = nome,
                Carga_Horaria = cargaHoraria,
                Valor = valor,
                Modulos = listaModulos
            };

            return View();
        }
    }
}
