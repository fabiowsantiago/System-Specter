using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specter_System.Models.Entitys
{
    public class PagSeguro
    {
        public string Email{ get; set; }
        public string Token { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string EmailComprador { get; set; }
    }
}
