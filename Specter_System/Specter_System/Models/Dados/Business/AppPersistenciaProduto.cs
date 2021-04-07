using Specter_System.Models.Dados.Classes;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using System.Collections.Generic;

namespace Specter_System.Models.Dados.Business
{
    public class AppPersistenciaProduto : ProdutoDAO, IPProduto
    {
        //Método para retornar a lista dos cursos por Modalidade para a View Index Home
        public List<Produto> Listar_Cursos(Produto model)
        {
            List<Produto> cursos = null;

            if ("Online".Equals(model.Modalidade))
                cursos = this.Select_Produtos_Online();

            else
                cursos = this.Listar_Cursos_Abertos(model);

            return cursos;
        }

        public bool CadastrarCurso(Produto curso)
        {
            bool resp = this.Insert(curso);

            return resp;
        }

        public bool CadastrarCursosOnline(Produto model)
        {
            bool resp = this.Insert_Produtos_Online(model);

            return resp;
        }

        //Método para pesquisar e listar os cursos por modalidades e exibir na tela de CADASTRO DE CURSOS
        public List<Produto> PesquisarCursosPorModalidade(Produto model)
        {
            List<Produto> cursos = this.PesquisarCursoPorModalidade(model);

            return cursos;
        }

        public bool AlterarCurso(Produto curso)
        {
            bool resp = this.Alterar_Curso(curso);

            return resp;
        }

        public Produto PesquisarCurso(Produto curso)
        {
            Produto course = this.Pesquisar_Curso(curso);

            return course;
        }

        public Produto SelectSobreProdutoOnline(Produto model)
        {
            Produto prod = this.Select_Sobre_Produto_Online(model);

            return prod;
        }

        public Produto Pesqusiar_Quantidade_Vagas(Produto model)
        {
            model = this.PesquisarVagasDisponiveis(model);

            return model;
        }

        public List<Produto> PesquisarCursos(Produto curso)
        {
            List<Produto> listCurso = this.Pesquisar_Cursos(curso);

            return listCurso;
        }

        public bool Update_Disponibilidade(Produto model)
        {
            bool resp = this.UpdateDisponibilidade(model);

            return resp;
        }

        public bool ExcluirCurso(Produto curso)
        {
            bool resp = this.Excluir_Curso(curso);

            return resp;
        }

        //Método para retornar o link do curriculo do(a) palestrante
        public string SelectLink(Produto model)
        {
            string link = this.Select_Link(model);

            return link;
        }

        public Produto SelectVideos(Produto model)
        {
            Produto produto = this.Select_Videos(model);

            return produto;
        }
    }
}
