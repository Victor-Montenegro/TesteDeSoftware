using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Core.Extensions;
using NerdStore.Vendas.Core.Interfaces;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Interfaces;

namespace NerdStore.Vendas.Application.Handlers
{
    public class AdicionarPedidoItemHandler : Core.DomainObject.RequestHandler<AdicionarItemPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public AdicionarPedidoItemHandler(IPedidoRepository pedidoRepository, IExchangeNotification exchangeNotification) : base(exchangeNotification)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async override Task<bool> Handle(AdicionarItemPedidoCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (! await ValidarCommand(command))
                    return false;

                PedidoItem pedidoItem;
                var pedido = await _pedidoRepository.ObterPedidoPorClienteId(command.ClienteId);

                if (pedido.IsNull())
                    (pedido, pedidoItem) = await AdicionarNovoPedido(command);
                else
                    (pedido, pedidoItem) = await AtualizarPedido(command, pedido);

                pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId,
                    pedidoItem.ProdutoId, pedido.Id, pedidoItem.Nome, pedidoItem.ValorUnitario, pedidoItem.Quantidade));

                var result = await _pedidoRepository.UnitOfWork.Commit();

                return result;
            }
            catch (Exception ex)
            {
                await _pedidoRepository.UnitOfWork.Rollback();

                throw;
            }
        }

        public async Task<(Pedido, PedidoItem)> AdicionarNovoPedido(AdicionarItemPedidoCommand command)
        {
            PedidoItemAdicionadoEvent evento;
            Pedido pedido = Pedido.PedidoFactory.CriarPedidoRascunho(command.ClienteId);
            PedidoItem pedidoItem = new PedidoItem(Guid.NewGuid(), command.ProdutoId, command.Name, command.Valor,
                command.Quantidade);

            pedido.AdicionarItem(pedidoItem);

            await _pedidoRepository.AdicionarPedido(pedido);

            return (pedido, pedidoItem);
        }

        public async Task<(Pedido, PedidoItem)> AtualizarPedido(AdicionarItemPedidoCommand command, Pedido pedido)
        {
            PedidoItem pedidoItem = new PedidoItem(Guid.NewGuid(), command.ProdutoId, command.Name, command.Valor,
                command.Quantidade);

            var pedidoItemExistente = pedido.ValidandoPedidoItemExistente(pedidoItem);

            pedido.AdicionarItem(pedidoItem);

            if (pedidoItemExistente)
            {
                var pedidoItemExistenteAtualizado =
                    pedido.PedidoItem.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
                await _pedidoRepository.AtualizarPedidoItem(pedidoItemExistenteAtualizado);
            }
            else
                await _pedidoRepository.AdicionarPedidoItem(pedidoItem);

            await _pedidoRepository.AtualizarPedido(pedido);

            return (pedido, pedidoItem);
        }
    }
}