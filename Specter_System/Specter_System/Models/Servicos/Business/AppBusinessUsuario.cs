using Specter_System.Models.Dados.Business;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Infaces;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Specter_System.Models.Servicos.Business
{
    public class AppBusinessUsuario : INUsuario
    {
        private IPUsuario appUsuario = new AppPersistenciaUsuario();

        public Usuario ValidarLogin(Usuario model)
        {
            bool respBloqueio = false;

            Usuario user = this.appUsuario.Validar_Login(model);

            if(user == null)
            {
                return user;
            }
            else if(user.Status == false)
            {
                Usuario usuario = new Usuario()
                {
                    Email = model.Email,
                    Status = true
                };
                respBloqueio = this.appUsuario.Bloquear_Login(usuario);
            }

            return user;
        }

        public bool SairLogin(Usuario model)
        {
            bool resp = this.appUsuario.Bloquear_Login(model);

            return resp;
        }

        public string Cadastrar_Usuario(Usuario model)
        {
            string resp = string.Empty;
            bool respVerificacao = false;

            if (!model.Senha.Equals(model.ConfirmarSenha))
            {
                resp = "Senhas nao conferem";
            }
            else if (!Regex.Match(model.Email, @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$").Success)
            {
                resp = "Informe um e-mail valido";
            }
            else
            {
                respVerificacao = this.appUsuario.Verificar_Email(model);

                if (respVerificacao == true) //Verifica se o e-mail informado pelo usuário já existe cadastrado no banco
                {
                    resp = "Email ja encontra-se cadastrado";
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

        public string RecuperarEmail(Usuario model)
        {
            string email = this.appUsuario.RecuperarEmail(model);

            return email;
        }

        public bool Enviar_Email(string emailDestinatario)
        {
            Random random = new Random();
            int cod = random.Next();
            string respEmail = string.Empty;
            bool resp = true;
            bool respCod = false;
            string emailRemetente = "info@fazendareal.com.br";

            MailMessage mail = new MailMessage();
            mail.To.Add(emailDestinatario);
            mail.From = new MailAddress(emailRemetente, "Fabio Welber Santiago", System.Text.Encoding.UTF8);

            mail.Subject = "Código para recuperação de senha";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Código para alteração de senha: " + Convert.ToString(cod);
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            //mail.Priority = MailPriority.High; //Prioridade do E-Mail

            SmtpClient client = new SmtpClient();  //Adicionando as credenciais do seu e-mail e senha:

            client.Credentials = new System.Net.NetworkCredential(emailRemetente, "ncrpln2020");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            try

            {
                client.Send(mail);

                Usuario model = new Usuario()
                {
                    Email = emailDestinatario,
                    codSenha = cod
                };

                respCod = this.Update_Cod_Senha(model);

                if (respCod == true)
                    return resp = true;
                else
                    return resp = false;

            }

            catch (Exception ex)

            {
                respEmail = "Ocorreu um erro ao enviar:" + ex.Message;

                return resp = false;
            }

        }

        private bool Update_Cod_Senha(Usuario model)
        {
            bool resp = this.appUsuario.UpdateCodSenha(model);

            return resp;
        }

        public string AlterarSenha(Usuario model)
        {
            string resp = string.Empty;
            bool respCod = false;

            if (!model.Senha.Equals(model.ConfirmarSenha))
                resp = "Senhas nao conferem";

            else
            {
                respCod = this.appUsuario.VerificarCodSenha(model);

                if (respCod == true)
                {
                    bool respUpdate = this.appUsuario.Update_Senha(model);

                    if (respUpdate == true)
                        resp = "Senha alterada";
                }
                else
                    resp = "Codigo invalido";

            }

            return resp;
        }
    }
}
