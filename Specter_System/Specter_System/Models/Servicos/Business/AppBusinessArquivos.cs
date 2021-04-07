using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Infaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace Specter_System.Models.Servicos.Business
{
    public class AppBusinessArquivos : Controller, INArquivo
    {
        public bool Salvar_Imagem_Curso_Online(Produto model) 
        {
            bool resp = false;
            var caminhoCurso = this.Caminho_Arquivo(model, "imagem");

            try
            {
                if (model.Imagem.ContentLength > 0 && model.Imagem_Palestrante.ContentLength > 0)
                {
                    model.Imagem.SaveAs(caminhoCurso);
                }

                resp = true;
            }
            catch (Exception error)
            {
                throw new Exception($"Error! {error.Message}");
            }

            return resp;
        }

        public string Salvar_Videos(Produto model)
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
                    }

                    resp = "video salvo";
                }
            }
            catch (Exception error)
            {
                throw new Exception($"Erro ao criar diretorio! {error.Message}");
            }
            return resp;
        }

        private string Caminho_Arquivo(Produto model, string tipoArquivo)
        {

            string pathCurso = string.Empty;
           

                var fileNameCurso = Path.GetFileName(model.Imagem.FileName);

                if ("Presencial".Equals(model.Modalidade))
                {
                    string pastaPresencial = "\\Models\\imagens\\cursos\\presenciais";
                    pathCurso = Path.Combine(pastaPresencial, fileNameCurso);
                }
                else
                {
                    string pastaOnline = "\\Models\\imagens\\cursos\\onlines";
                    pathCurso = Path.Combine(pastaOnline, fileNameCurso);
                }
     
            return pathCurso;
        }
    }
}
