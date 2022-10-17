using System;
using System.Collections.Generic;
using System.Linq;
using NerdStore.Vendas.Core.Data;
using NerdStore.Vendas.Core.Interfaces;
using NerdStore.Vendas.Domain.Enums;
using NerdStore.Vendas.Domain.Validatories;

namespace NerdStore.Vendas.Domain.Entities
{
    public class Pedido : BaseEntity,IAggregateRoot
    {
        private List<PedidoItem> _pedidoItems;

        public Guid ClienteId { get; private set; }
        public Voucher Voucher { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public PedidoStatus Status { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public IReadOnlyCollection<PedidoItem> PedidoItem => _pedidoItems;

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public bool ValidandoPedidoItemExistente(PedidoItem pedidoItem)
            => _pedidoItems.Any(a => a.ProdutoId == pedidoItem.ProdutoId);

        public Pedido AdicionarItem(PedidoItem pedidoItem)
        {
            if (ValidandoPedidoItemExistente(pedidoItem))
                pedidoItem = AtualizarItemExistente(pedidoItem);

            _pedidoItems.Add(pedidoItem);

            TornarRascunho();

            AtualizarValorTotal();

            return this;
        }

        private PedidoItem AtualizarItemExistente(PedidoItem pedidoItem)
        {
            ChecarItemPedidoExistente(pedidoItem);

            int quantidade = pedidoItem.Quantidade;
            var pedidoItemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);

            pedidoItemExistente.AtualizarQuantidade(quantidade);

            pedidoItemExistente.ChecandoQuantidade();

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
            
            AplicarDesconto();
        }

        public void TornarRascunho()
        {
            Status = PedidoStatus.Rascunho;
        }

        private void ChecarItemPedidoExistente(PedidoItem pedidoItem)
        {
            if (!_pedidoItems.Any(a => a.ProdutoId == pedidoItem.ProdutoId))
                throw new DomainException($"O item {pedidoItem.Nome} não existe para ser atualizado");
        }

        public Pedido RemoveItem(PedidoItem pedidoItem)
        {
            ChecarItemPedidoExistente(pedidoItem);

            _pedidoItems.Remove(pedidoItem);

            AtualizarValorTotal();

            return this;
        }

        public bool ValidarPedido()
        {
            var validator = new PedidoValidator();
            ValidationResult = validator.Validate(this);

            return ValidationResult.IsValid;
        }

        public bool AplicarVoucher(Voucher voucher)
        {
            voucher.ValidarVoucher();
            ValidationResult = voucher.ValidationResult;

            if (ValidationResult.IsValid)
            {
                Voucher = voucher;
                VoucherUtilizado = true;

                AplicarDesconto();
            }

            return ValidationResult.IsValid;
        }

        private void AplicarDesconto()
        {
            if (!VoucherUtilizado)
                return;
            
            if(Voucher.Tipo == TipoDescontoVoucher.Valor)
                CalcularDescontoValor();
            
            if (Voucher.Tipo == TipoDescontoVoucher.Porcentual)
                CalcularDescontoPercentual();

            decimal CalculoValorTotalComDesconto = ValorTotal - Desconto;
            
            ValorTotal = CalculoValorTotalComDesconto < 0 ? 0 : CalculoValorTotalComDesconto;
        }

        private void CalcularDescontoPercentual()
        {
            if (Voucher.PercentualDesconto.HasValue)
                Desconto = (ValorTotal * Voucher.PercentualDesconto.Value) / 100;
        }

        private void CalcularDescontoValor()
        {
            if (Voucher.ValorDesconto.HasValue)
                Desconto = Voucher.ValorDesconto.Value;
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
    }
}