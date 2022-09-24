using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Interfaces;

namespace NerdStore.Vendas.Application.Handlers
{
    public class AdicionarPedidoItemHandler : IRequestHandler<AdicionarItemPedidoRequest,AdicionarItemPedidoResponse>
    {
        private readonly IPedidoRepository _pedidoRepository;
        
        public AdicionarPedidoItemHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }
        
        public async Task<AdicionarItemPedidoResponse> Handle(AdicionarItemPedidoRequest request, CancellationToken cancellationToken)
        {
            PedidoItemAdicionadoEvent evento;
            Pedido pedido = Pedido.PedidoFactory.CriarPedidoRascunho(request.ClienteId);
            PedidoItem pedidoItem = new PedidoItem(request.ProdutoId, request.Name, request.Valor, request.Quantidade);
                
            pedido.AdicionarItem(pedidoItem);

            try
            {
                await _pedidoRepository.AdicionarPedido(pedido);

                pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, 
                    pedidoItem.ProdutoId, pedido.Id, pedidoItem.Nome,pedidoItem.ValorUnitario,pedidoItem.Quantidade));

                await _pedidoRepository.UnitOfWork.Commit();
            }
            catch (Exception)
            {
                await _pedidoRepository.UnitOfWork.Rollback();
                
                throw;
            }
            
            var response = CreateResponseSuccess(pedido);
            
            return response;
        }

        public AdicionarItemPedidoResponse CreateResponseSuccess(Pedido pedido)
            => new()
            {
                Success = true,
                Pedido = pedido
            };
    }
}