using Specter_System.Models.Entitys;

namespace Specter_System.Models.Servicos.Infaces
{
    public interface INUsuario
    {
        Usuario ValidarLogin(Usuario model);
        string Cadastrar_Usuario(Usuario model);
    }
}
