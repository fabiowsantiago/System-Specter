using Specter_System.Models.Entitys;
using System.Collections.Generic;

namespace Specter_System.Models.Dados.Interfaces
{
    public interface IPProduto
    {
        List<Produto> Listar_Cursos(Produto curso);
        bool CadastrarCurso(Produto curso);
        bool CadastrarCursosOnline(Produto model);
        List<Produto> PesquisarCursosPorModalidade(Produto model);
        Produto Pesqusiar_Quantidade_Vagas(Produto model);
        bool AlterarCurso(Produto curso);
        bool ExcluirCurso(Produto curso);
        bool Update_Disponibilidade(Produto model);
        Produto PesquisarCurso(Produto curso);
        Produto SelectSobreProdutoOnline(Produto model);
        List<Produto> PesquisarCursos(Produto curso);
        string SelectLink(Produto model);
        Produto SelectVideos(Produto model);
    }
}
