using Specter_System.Models.Dados.Classes;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;

namespace Specter_System.Models.Dados.Business
{
    public class AppPersistenciaUsuario : UsuarioDAO, IPUsuario
    {
        public Usuario Validar_Login(Usuario model)
        {
            Usuario user = this.ValidarLogin(model);

            return user;
        }

        public bool Insert_Usuario(Usuario model)
        {
            bool resp = this.InsertUsuario(model);

            return resp;
        }

        public string RecuperarEmail(Usuario model)
        {
            string email = this.Recuperar_Email(model);

            return email;
        }

        public bool UpdateCodSenha(Usuario model)
        {
            bool resp = this.Update_Cod_Senha(model);

            return resp;
        }

        public bool VerificarCodSenha(Usuario model)
        {
            bool resp = this.Verificar_Cod_Senha(model);

            return resp;
        }

        public bool UpdateSenha(Usuario model)
        {
            bool resp = this.Update_Senha(model);

            return resp;
        }

        public bool Verificar_Email(Usuario model)
        {
            bool resp = this.VerificarEmail(model);

            return resp;
        }

        public bool Bloquear_Login(Usuario model)
        {
            bool resp = this.BloquearLogin(model);

            return resp;
        }

        public bool Desbloquear_Login(Usuario model)
        {
            bool resp = this.DesbloquearLogin(model);

            return resp;
        }
    }
}
