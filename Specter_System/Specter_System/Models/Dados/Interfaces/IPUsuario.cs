using Specter_System.Models.Entitys;

namespace Specter_System.Models.Dados.Interfaces
{
    public interface IPUsuario
    {
        Usuario Validar_Login(Usuario model);
        bool Insert_Usuario(Usuario model);
        bool Verificar_Email(Usuario model);
    }
}
