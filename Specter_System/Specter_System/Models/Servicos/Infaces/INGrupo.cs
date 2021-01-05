using Specter_System.Models.Entitys;

namespace Specter_System.Models.Servicos.Infaces
{
    public interface INGrupo
    {
        string Cadastrar(Grupo model);
        Grupo ListarGrupos();
        Grupo Integrar(Grupo model);
    }
}
