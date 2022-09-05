using System;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Helpers;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTest
    {
        [Fact(DisplayName = "Adicionar unidade de item de pedido acima do permitido")]
        [Trait("PedidoItem", "Pedido Tests")]
        public void PedidoItem_AdicininarUnidadeItemPedidoAcimaPermitido_DeveRetornaException()
        {
            //Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(),
                "iphone 10",
                300,
                PedidoItemHelper.MAX_UNIDADE_PRODUTO + 1));
        }

        [Fact(DisplayName = "Adicionar unidade de item de pedido abaixo do permitido")]
        [Trait("PedidoItem", "Pedido Tests")]
        public void PedidoItem_AdicininarUnidadeItemPedidoAbaixoPermitido_DeveRetornaException()
        {
            //Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(),
                "iphone 10",
                300,
                PedidoItemHelper.MIN_UNIDADE_PRODUTO - 1));
        }
    }
}