using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specter_System.Models.Entitys
{
    public class Pedidos
    {
        public string Curso { get; set; }
        public string CargaHoraria { get; set; }
        public List<Produto> Produtos { get; set; }
        public List<string> ListPedido { get; set; }
        
    }
}
