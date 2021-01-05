using Specter_System.Models.Entitys;
using System.Collections.Generic;

namespace Specter_System.Models.Servicos.Infaces
{
    public interface INProduto
    {
        List<Produto> ListarCursos(Produto curso);
        Produto CadastrarCurso(Produto curso);
        List<Produto> PesquisarCursosPorModalidade(Produto model);
        Produto AlterarCurso(Produto curso);
        Produto ExcluirCurso(Produto model);
        Produto PesquisarCurso(Produto curso);
        List<Produto> PesquisarCursos(Produto curso);
    }
}
