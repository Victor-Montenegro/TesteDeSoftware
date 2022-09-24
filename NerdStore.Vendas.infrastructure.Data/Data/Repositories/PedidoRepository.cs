using System.Threading.Tasks;
using NerdStore.Vendas.Core.Data;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Interfaces;

namespace NerdStore.Vendas.infrastructure.Data.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        public Task AdicionarPedido(Pedido pedido)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public IUnitOfWork UnitOfWork { get; }
    }
}