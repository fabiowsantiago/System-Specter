using System.Collections.Generic;

namespace Specter_System.Models.Entitys
{
    public class Pedido
    {
        public int Cod { get; set; }
        public string Data { get; set; }
        public string Horario { get; set; }
        public Pessoa Pessoa { get; set; }
        public string TipoInscricao { get; set; }
        public Grupo Grupo { get; set; }
        public List<Grupo> Grupos { get; set; }
        public string ValorPagamento { get; set; }
        public string Resposta { get; set; }
        public Produto Produto { get; set; }
    }
}
