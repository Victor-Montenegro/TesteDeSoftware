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
        [Trait("Pedido", "Vendas - Pedidos")]
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
        [Trait("Pedido", "Vendas - Pedidos")]
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
        [Trait("Pedido", "Vendas - Pedidos")]
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
        [Trait("Pedido", "Vendas - Pedido")]
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
        [Trait("Pedido", "Vendas - Pedido")]
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
        
        [Fact(DisplayName = "Atualizando um item de pedido")]
        [Trait("Pedido", "Vendas - Pedido")]
        public void Pedido_AtualizarPedidoItemValido_DeveAtualizarQuantidade()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Samsung S22", 2000, 2);
            var pedidoItemExistente = new PedidoItem(produtoId, "Samsung S22", 2000, 10);
            var novaQuantidade = pedidoItemExistente.Quantidade;
            
            pedido.AdicionarItem(pedidoItem);
            
            //Act
            pedido.AtualizarItem(pedidoItemExistente);
            
            // Assert
            Assert.Equal(novaQuantidade,pedido.PedidoItem.FirstOrDefault(x => x.ProdutoId == produtoId).Quantidade);
        }
        
        [Fact(DisplayName = "Adicionando um item de pedido existente com quantidade acima do permitido")]
        [Trait("Pedido", "Vendas - Pedido")]
        public void Pedido_AdicionandoPedidoItemExistenteComQuantidadeAcimaDoPermitido_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Samsung S22", 2000, 2);
            var pedidoItemExistente = new PedidoItem(produtoId, "Samsung S22", 2000, PedidoItemHelper.MAX_UNIDADE_PRODUTO);

            pedido.AdicionarItem(pedidoItem);
            
            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItemExistente));
        }

        [Fact(DisplayName = "Atualizar pedido item não existente")]
        [Trait("Pedido", "Vendas - Pedido")]
        public void Pedido_AtualizarPedidoItemNaoExistente_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Item nao existente", 10, 10);
            
            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItem));
        }

        [Fact(DisplayName = "Adicionando e atualizando um pedido item existente")]
        [Trait("Pedido", "Vendas - Pedido")]
        public void Pedido_AdicionandoAtualizandoPedidoItens_DeveAtualizarValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(Guid.NewGuid());
            var produtoId = Guid.NewGuid();
            var pedidoItemIphone = new PedidoItem(Guid.NewGuid(), "Iphone 13", 1000, 1);
            var pedidoItemCarregador = new PedidoItem(produtoId, "Carregador iphone", 500, 1);
            var pedidoItemCarregadorAtualizado = new PedidoItem(produtoId, "Carregador iphone", 500, 3);
            var valorTotalEsperado = pedidoItemIphone.Quantidade * pedidoItemIphone.ValorUnitario +
                                     pedidoItemCarregadorAtualizado.Quantidade * pedidoItemCarregadorAtualizado.ValorUnitario;
            
            pedido.AdicionarItem(pedidoItemIphone);
            pedido.AdicionarItem(pedidoItemCarregador);
            
            //Act
            pedido.AtualizarItem(pedidoItemCarregadorAtualizado);

            //Assert
            Assert.Equal(valorTotalEsperado, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Removendo item não existente")]
        [Trait("Pedido", "Vendas - Pedido")]
        public void Pedido_RemovendoItemNaoExistente_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(Guid.NewGuid());
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Pedido nao existente", 10, 10);

            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.RemoveItem(pedidoItem));
        }

        [Fact(DisplayName = "Remover pedido item existente, pedido deve atualziar valor")]
        [Trait("Pedido", "Vendas - Pedido")]
        public void Pedido_RemoverItemExistente_DeveAtualizarValorTotal()
        {
            //Arrange
            var produtoId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(Guid.NewGuid());
            var pedidoItemSamsung = new PedidoItem(produtoId, "Samsung S10", 1000, 1);
            var pedidoItemCarregador = new PedidoItem(Guid.NewGuid(), "Carregador samsung", 100, 1);
            var valorTotalEsperado = pedidoItemSamsung.Quantidade * pedidoItemSamsung.ValorUnitario;

            pedido.AdicionarItem(pedidoItemSamsung);
            pedido.AdicionarItem(pedidoItemCarregador);
            
            //Act
            pedido.RemoveItem(pedidoItemCarregador);
            
            //Assert
            Assert.Equal(valorTotalEsperado,pedido.ValorTotal);
            Assert.True(pedido.PedidoItem.Count(x => x.ProdutoId != produtoId) == 0);
        }
    }
}