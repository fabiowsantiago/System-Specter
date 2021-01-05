using Specter_System.Models.Dados.Classes;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using System.Collections.Generic;

namespace Specter_System.Models.Dados.Business
{
    public class AppPersistenciaProduto : ProdutoDAO, IPProduto
    {
        public List<Produto> ListarCursosAbertos(Produto curso)
        {
            List<Produto> cursos = this.Listar_Cursos_Abertos(curso);

            return cursos;
        }

        public bool CadastrarCurso(Produto curso)
        {
            bool resp = this.Insert(curso);

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

        public Produto Pesqusiar_Quantidade_Vagas(Produto model)
        {
            Produto curso = this.PesquisarVagasDisponiveis(model);

            return curso;
        }

        public List<Produto> PesquisarCursos(Produto curso)
        {
            List<Produto> listCurso = this.Pesquisar_Cursos(curso);

            return listCurso;
        }

        public bool ExcluirCurso(Produto curso)
        {
            bool resp = this.Excluir_Curso(curso);

            return resp;
        }
    }
}
