using Specter_System.Models.Dados.Classes;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool Verificar_Email(Usuario model)
        {
            bool resp = this.VerificarEmail(model);

            return resp;
        }
    }
}
