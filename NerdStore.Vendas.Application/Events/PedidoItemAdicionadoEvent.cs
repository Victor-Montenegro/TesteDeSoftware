using System;
using NerdStore.Vendas.Core.Messages;

namespace NerdStore.Vendas.Application.Events
{
    public class PedidoItemAdicionadoEvent : Event
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public string ProdutoNome { get; set; }
        public decimal ValorUnitario { get; set; }

        public PedidoItemAdicionadoEvent(Guid clienteId, Guid produtoId, Guid pedidoId, string produtoNome, decimal valorUnitario, int quantidade)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            PedidoId = pedidoId;
            ProdutoNome = produtoNome;
            ValorUnitario = valorUnitario;
            Quantidade = quantidade;
        }
    }
}