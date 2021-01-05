using Specter_System.Models.Dados.Business;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Infaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specter_System.Models.Servicos.Business
{
    public class AppBusinessUsuario : INUsuario
    {
        private IPUsuario appUsuario = new AppPersistenciaUsuario();

        public Usuario ValidarLogin(Usuario model)
        {
            Usuario user = this.appUsuario.Validar_Login(model);

            return user;
        }

        public string Cadastrar_Usuario(Usuario model)
        {
            string resp = string.Empty;
            bool respVerificacao = false;

            if (!model.Senha.Equals(model.ConfirmarSenha))
            {
                resp = "Senhas não conferem";
            }
            else
            {
                respVerificacao = this.appUsuario.Verificar_Email(model);

                if (respVerificacao == true) //Verifica se o e-mail informado pelo usuário já existe cadastrado no banco
                {
                    resp = "Email já encontra-se cadastrado";
                }
                else
                {
                    respVerificacao = this.appUsuario.Insert_Usuario(model);

                    if (respVerificacao == true)
                    {
                        resp = "Cadastrado";
                    }
                    else
                    {
                        resp = "Erro ao realizar cadastrado";
                    }
                }
            }

            return resp;
        }
    }
}
