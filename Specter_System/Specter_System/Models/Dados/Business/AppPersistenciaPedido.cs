using Specter_System.Models.Dados.Classes;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;

namespace Specter_System.Models.Dados.Business
{
    public class AppPersistenciaPedido : PedidoDAO, IPPedido
    {
        public bool Insert_Pedido_Grupo(Pedido model)
        {
            bool resp = this.Insert_Grupo(model);

            return resp;
        }

        public Pedidos PesquisarPedidosOnline(Pedido model)
        {
            Pedidos pedidos = this.Pesquisar_Pedidos_Online(model);

            return pedidos;
        }
    }
}
