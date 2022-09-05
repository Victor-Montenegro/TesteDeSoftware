using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation.Results;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Enums;
using NerdStore.Vendas.Domain.Helpers;
using NerdStore.Vendas.Domain.Validatories;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionando novo Item Pedido")]
        [Trait("Pedido", "Pedido Tests")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(Guid.Empty);
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto teste", 100, 2);

            //Act
            pedido.AdicionarItem(pedidoItem);

            //Assert
            Assert.Equal(200, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar item existente")]
        [Trait("Pedido", "Pedido Tests")]
        public void PedidoItem_AdicionarPedidoExistente_DeveAtualizarQuantidadePedidoItemExistenteDeveAtualizarValor()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(Guid.Empty);
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto teste", 100, 2);
            var pedidoItemExistente = new PedidoItem(produtoId, "Produto teste", 100, 5);

            //Act
            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(pedidoItemExistente);

            //Assert
            Assert.True(pedido.PedidoItem.Count() == 1);
            Assert.Equal(700, pedido.ValorTotal);
            Assert.Equal(7, pedido.PedidoItem.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Adicionando pedido que deve permanecer como rascunho ")]
        [Trait("Pedido", "Pedido Tests")]
        public void Pedido_AdicionandoPedidoRascunho_DevePermanecerRascunhoTerIdentificacaoCliente()
        {
            //Arrange
            var clienteId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(clienteId);
            var novoPedidoItem = new PedidoItem(Guid.NewGuid(), "Teste rascunho", 100, 2);

            //Act
            pedido.AdicionarItem(novoPedidoItem);

            //Assert
            Assert.True(pedido.Status == PedidoStatus.Rascunho);
            Assert.Equal(clienteId, pedido.ClienteId);
        }

        [Fact(DisplayName = "Adicionando um item de pedido existente no pedido")]
        [Trait("Pedido", "Pedido Test")]
        public void Pedido_AdicionandoItemExistentePedido_DeveAtualizarQuantidadeEValorPedido()
        {
            //Arrange
            var clienteId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(clienteId);
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Samsung S22", 2000, 2);
            var pedidoItemExistente = new PedidoItem(produtoId, "Samsung S22", 2000, 3);
            var quantidadeItemPedido = pedidoItemExistente.Quantidade;
            pedido.AdicionarItem(pedidoItem);

            //Act
            pedido.AtualizarItem(pedidoItemExistente);

            //Assert
            Assert.Equal(6000, pedido.ValorTotal);
            Assert.Equal(quantidadeItemPedido, pedido.PedidoItem.FirstOrDefault().Quantidade);
        }

        [Fact(DisplayName = "Atualizando um item de pedido com quantidade acime do permitido")]
        [Trait("Pedido", "Pedido Test")]
        public void Pedido_AtualizarItemPedidoQuantidadeAcimaPermitido_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Samsung S22", 2000, 2);
            // var pedidoItemExistente = new PedidoItem(produtoId, "Samsung S22", 2000, PedidoItemHelper.MAX_UNIDADE_PRODUTO + 1);

            pedido.AdicionarItem(pedidoItem);
            pedidoItem.AtualizarQuantidade(PedidoItemHelper.MAX_UNIDADE_PRODUTO + 1);

            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItem));
        }
    }
}