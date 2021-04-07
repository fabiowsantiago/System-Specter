using Specter_System.Models.Entitys;

namespace Specter_System.Models.Dados.Interfaces
{
    public interface IPPedido
    {
        bool Insert_Pedido_Grupo(Pedido model);
        Pedidos PesquisarPedidosOnline(Pedido model);
    }
}
