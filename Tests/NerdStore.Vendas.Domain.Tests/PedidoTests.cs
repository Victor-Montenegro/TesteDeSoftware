using System;
using System.Linq;
using FluentAssertions;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Enums;
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
            var pedidoItem = new PedidoItem(Guid.NewGuid(),"Produto teste", 2, 100);

            //Act

            pedido.AdicionarItem(pedidoItem);

            // pedido.AtualizarValor();

            //Assert
            Assert.Equal(200,pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar item existente")]
        [Trait("Pedido", "Pedido Tests")]
        public void PedidoItem_AdicionarPedidoExistente_DeveAtualizarQuantidadePedidoItemExistenteDeveAtualizarValor()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(Guid.Empty);
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId,"Produto teste", 100, 2); 
            var pedidoItemExistente = new PedidoItem(produtoId,"Produto teste", 100, 5); 
            
            //Act
            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(pedidoItemExistente);
            //Assert
            // Assert.Equal(7,pedido.PedidoItem
            //     .Where(w => w.Id == pedidoItem.Id)
            //     .Select(s => s.Quantidade)
            //     .First());
            
            Assert.True(pedido.PedidoItem.Count() == 1);
            Assert.Equal(700,pedido.ValorTotal);
            Assert.Equal(7,pedido.PedidoItem.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Adicionando pedido que deve permanecer como rascunho ")]
        [Trait("Pedido","Pedido Tests")]
        public void Pedido_AdicionandoPedidoRascunho_DevePermanecerRascunhoTerIdentificacaoCliente()
        {
            //Arrange
            var clienteId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.CriarPedidoRascunho(clienteId);
            var novoPedidoItem = new PedidoItem(Guid.NewGuid(), "Teste rascunho", 100,2);
            //Act
            pedido.AdicionarItem(novoPedidoItem);
            
            //Assert
            Assert.True(pedido.Status == PedidoStatus.Rascunho);
        }
    }
}