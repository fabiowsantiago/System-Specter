using Specter_System.Models.Entitys;

namespace Specter_System.Models.Dados.Interfaces
{
    public interface IPUsuario
    {
        Usuario Validar_Login(Usuario model);
        bool Insert_Usuario(Usuario model);
        string RecuperarEmail(Usuario model);
        bool Verificar_Email(Usuario model);
        bool VerificarCodSenha(Usuario model);
        bool UpdateSenha(Usuario model);
        bool UpdateCodSenha(Usuario model);
        bool Bloquear_Login(Usuario model);
        bool Desbloquear_Login(Usuario model);
        bool Update_Senha(Usuario model);

    }
}
