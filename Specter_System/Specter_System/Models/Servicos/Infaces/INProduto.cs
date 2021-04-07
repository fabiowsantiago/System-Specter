using Specter_System.Models.Entitys;
using System.Collections.Generic;

namespace Specter_System.Models.Servicos.Infaces
{
    public interface INProduto
    {
        List<Produto> ListarCursos(Produto curso);
        Produto CadastrarCurso(Produto curso);
        string CadastrarCursoOnline(Produto model);
        List<Produto> PesquisarCursosPorModalidade(Produto model);
        Produto AlterarCurso(Produto curso);
        Produto ExcluirCurso(Produto model);
        Produto PesquisarCurso(Produto curso);
        Produto Pesquisar_Sobre_Produto_Online(Produto model);
        List<Produto> PesquisarCursos(Produto curso);
        string Pesquisar_Link(Produto model);
        Produto PesquisarVideos(Produto model);
    }
}
