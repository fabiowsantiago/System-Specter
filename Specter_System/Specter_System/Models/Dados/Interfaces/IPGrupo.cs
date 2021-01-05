using Specter_System.Models.Entitys;

namespace Specter_System.Models.Dados.Interfaces
{
    public interface IPGrupo
    {
        Grupo ListarNomeGrupo();
        bool PesquisarGrupo(Grupo model);
        bool Cadastrar(Grupo model);
        Grupo PesquisarQtdComponentes(Grupo model);
    }
}
