using Specter_System.Models.Entitys;
using System.Collections.Generic;

namespace Specter_System.Models.Dados.Interfaces
{
    public interface IPProduto
    {
        List<Produto> ListarCursosAbertos(Produto curso);
        bool CadastrarCurso(Produto curso);
        List<Produto> PesquisarCursosPorModalidade(Produto model);
        Produto Pesqusiar_Quantidade_Vagas(Produto model);
        bool AlterarCurso(Produto curso);
        bool ExcluirCurso(Produto curso);
        Produto PesquisarCurso(Produto curso);
        List<Produto> PesquisarCursos(Produto curso);
    }
}
