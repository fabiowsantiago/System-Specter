using Specter_System.Models.Dados.Business;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Infaces;
using System.Collections.Generic;

namespace Specter_System.Models.Servicos.Business
{
    public class AppBusinessProduto : INProduto
    {
        private IPProduto appProduto = new AppPersistenciaProduto();
        public List<Produto> ListarCursos(Produto curso)
        {
            List<Produto> cursos = appProduto.ListarCursosAbertos(curso);

            return cursos;
        }

        public Produto CadastrarCurso(Produto curso)
        {
            bool respCad = this.appProduto.CadastrarCurso(curso);

            if (respCad == true)
            {
                curso = new Produto()
                {
                    LabResp = "Curso cadastrado com sucesso"
                };
            }
            else
            {
                curso = new Produto()
                {
                    LabResp = "Erro ao realizar cadastro"
                };
            }

            return curso;
        }

        public List<Produto> PesquisarCursosPorModalidade(Produto model)
        {
            List<Produto> cursos = this.appProduto.PesquisarCursosPorModalidade(model);

            return cursos;
        }

        public Produto AlterarCurso(Produto curso)
        {
            bool resp = this.appProduto.AlterarCurso(curso);

            if (resp == true)
            {
                curso = new Produto()
                {
                    LabResp = "Curso alterado com sucesso"
                };
            }
            else
            {
                curso = new Produto()
                {
                    LabResp = "Erro ao alterar informações do curso"
                };
            }

            return curso;
        }

        public Produto ExcluirCurso(Produto model)
        {
            Produto curso = null;

            bool resp = this.appProduto.ExcluirCurso(model);

            if (resp == true)
            {
                curso = new Produto()
                {
                    LabResp = "Curso excluído com sucesso"
                };
            }
            else
            {
                curso = new Produto()
                {
                    LabResp = "Erro ao excluir Curso"
                };
            }

            return curso;
        }

        public Produto PesquisarCurso(Produto curso)
        {
            curso = this.appProduto.PesquisarCurso(curso);

            if (curso == null)
            {
                curso = new Produto()
                {
                    LabResp = "Curso não localizado"
                };
            }

            return curso;
        }

        public List<Produto> PesquisarCursos(Produto curso)
        {
            List<Produto> listCursos = this.appProduto.PesquisarCursos(curso);

            return listCursos;
        }
    }
}
