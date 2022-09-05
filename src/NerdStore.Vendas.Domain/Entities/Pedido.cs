using System;
using System.Collections.Generic;
using System.Linq;
using NerdStore.Vendas.Domain.Enums;

namespace NerdStore.Vendas.Domain.Entities
{
    public class Pedido : BaseEntity
    {
        private List<PedidoItem> _pedidoItems;

        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public PedidoStatus Status { get; private set; }
        public IReadOnlyCollection<PedidoItem> PedidoItem => _pedidoItems;

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public Pedido AdicionarItem(PedidoItem pedidoItem)
        {
            if (_pedidoItems.Any(a => a.Id == pedidoItem.Id))
                pedidoItem = AtualizarItemExistente(pedidoItem);

            _pedidoItems.Add(pedidoItem);

            _pedidoItems.ForEach(x => x.ChecandoQuantidade());

            Status = PedidoStatus.Rascunho;

            AtualizarValorTotal();

            return this;
        }

        private PedidoItem AtualizarItemExistente(PedidoItem pedidoItem)
        {
            ChecarItemPedidoExistente(pedidoItem);

            int quantidade = pedidoItem.Quantidade;
            var pedidoItemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);

            pedidoItemExistente.AtualizarQuantidade(quantidade);

            _pedidoItems.Remove(pedidoItemExistente);

            return pedidoItemExistente;
        }

        public Pedido AtualizarItem(PedidoItem pedidoItem)
        {
            ChecarItemPedidoExistente(pedidoItem);
            
            pedidoItem.ChecandoQuantidade();

            var pedidoExistente = _pedidoItems.FirstOrDefault(x => x.ProdutoId == pedidoItem.ProdutoId);

            _pedidoItems.Remove(pedidoExistente);
            _pedidoItems.Add(pedidoItem);
            
            AtualizarValorTotal();
            
            return this;
        }

        private void AtualizarValorTotal()
        {
            decimal valorAtualizado = PedidoItem.Sum(s => s.CalcularValor());

            ValorTotal = valorAtualizado;
        }

        public void TornarRascunho()
        {
            Status = PedidoStatus.Rascunho;
        }

        public static class PedidoFactory
        {
            public static Pedido CriarPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido()
                {
                    ClienteId = clienteId
                };

                pedido.TornarRascunho();

                return pedido;
            }
        }

        public void ChecarItemPedidoExistente(PedidoItem pedidoItem)
        {
            if (!_pedidoItems.Any(a => a.Id == pedidoItem.Id))
                throw new DomainException($"O item {pedidoItem.Nome} não existe para ser atualizado");
        }
        // public override bool EhValido()
        // {
        //     var validator = new PedidoValidator();
        //     var validation = validator.Validate(this);
        //
        //     if (validation.IsValid)
        //         return true;
        //
        //     ValidationResult = validation;
        //
        //     return false;
        // }
    }
}