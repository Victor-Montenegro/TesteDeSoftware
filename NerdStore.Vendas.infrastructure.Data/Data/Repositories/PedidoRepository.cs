using System;
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

        public Task AtualizarPedido(Pedido pedido)
        {
            throw new NotImplementedException();
        }

        public Task AdicionarPedidoItem(PedidoItem pedido)
        {
            throw new NotImplementedException();
        }

        public Task AtualizarPedidoItem(PedidoItem pedidoItem)
        {
            throw new NotImplementedException();
        }

        public Task<Pedido> ObterPedidoPorClienteId(Guid pedido)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public IUnitOfWork UnitOfWork { get; }
    }
}