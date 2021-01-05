using Specter_System.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specter_System.Models.Dados.Interfaces
{
    public interface IPPessoa
    {
        bool Cadastrar(Pessoa model);
        bool Pesquisar_Cpf(Pessoa model);
    }
}
