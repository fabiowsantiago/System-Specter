using Specter_System.Models.Dados.Classes;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;

namespace Specter_System.Models.Dados.Business
{
    public class AppPersistenciaPessoa : PessoaDAO, IPPessoa
    {
        public bool Cadastrar(Pessoa model)
        {
            bool resp = this.Insert(model);
            return resp;
        }

        public bool Pesquisar_Cpf(Pessoa model)
        {
            bool resp = this.PesquisarCpf(model);

            return resp;
        }
    }
}
