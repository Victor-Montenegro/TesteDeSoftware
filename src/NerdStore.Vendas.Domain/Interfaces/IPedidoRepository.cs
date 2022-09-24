using System.Threading.Tasks;
using NerdStore.Vendas.Core.Data;
using NerdStore.Vendas.Domain.Entities;

namespace NerdStore.Vendas.Domain.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task AdicionarPedido(Pedido pedido);
    }
}