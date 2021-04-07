using Specter_System.Models.Entitys;

namespace Specter_System.Models.Servicos.Infaces
{
    public interface INPedido
    {
        string RealizarIncricao(Pedido model);
        Pedidos Pesquisar_Pedidos_Online(Pedido model);
    }
}
