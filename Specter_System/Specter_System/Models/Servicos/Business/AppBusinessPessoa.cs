using Specter_System.Models.Dados.Business;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Infaces;

namespace Specter_System.Models.Servicos.Business
{
    public class AppBusinessPessoa : INPessoa
    {
        private IPPessoa appPessoa = new AppPersistenciaPessoa();

        public string CadastrarPessoa(Pessoa model)
        {
            string resposta = string.Empty;
            bool resp = false;
            bool validarCpf = false;

            validarCpf = this.ValidarCPF(model.CPF);

            if (validarCpf == true)
            {
                resp = this.appPessoa.Pesquisar_Cpf(model);

                if (resp == true)
                {
                    resposta = "CPF ja encontra-se cadastrado";
                }

                else
                {
                    resp = this.appPessoa.Cadastrar(model);

                    if (resp == true)

                        resposta = "Cadastrado";
                    else
                        resposta = "Erro ao realizar Cadastro";
                }

            }
            else
            {
                resposta = "Cpf informado nao e valido";
            }

            return resposta;
        }

        private bool ValidarCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
