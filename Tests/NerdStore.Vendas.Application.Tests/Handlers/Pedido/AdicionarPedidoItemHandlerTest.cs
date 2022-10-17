using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Handlers;
using NerdStore.Vendas.Core.Data;
using NerdStore.Vendas.Core.Interfaces;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Interfaces;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Handlers.Pedido
{
    public class AdicionarPedidoItemHandlerTest
    {
        [Fact(DisplayName = "")]
        [Trait("", "")]
        public async void AdicionarPedidoItemHandler_NovoPedidoItem_DeveRetonarSucesso()
        {
            //Arrange
            var autoMock = new AutoMocker();
            var pedidoHandler = autoMock.CreateInstance<AdicionarPedidoItemHandler>();
            var request = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(),"Samsung S10", 1000, 1);

            autoMock.GetMock<IPedidoRepository>().Setup(p => p.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));
            
            //Act
            var response = await pedidoHandler.Handle(request, CancellationToken.None);
            
            //Assert
            Assert.True(response);
            autoMock.GetMock<IPedidoRepository>().Verify(p => p.AdicionarPedido(It.IsAny<Domain.Entities.Pedido>()),Times.Once);
            // autoMock.GetMock<IMediator>().Verify(p => p.Publish(It.IsAny<INotification>(),CancellationToken.None),Times.Once);
        }

        [Fact()]
        [Trait("","")]
        public async void AdicionarPedidoItemHandler_AdicionandoPedidoExistente_DeveAtualizarPedido()
        {
            //Arrange
            var clienteId = Guid.NewGuid();
            var automocker = new AutoMocker();
            var pedidoHandler = automocker.CreateInstance<AdicionarPedidoItemHandler>();
            var pedidoExistente = Domain.Entities.Pedido.PedidoFactory.CriarPedidoRascunho(clienteId);
            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId, Guid.NewGuid(),"Samsung S10", 1000, 1);

            automocker.GetMock<IPedidoRepository>().Setup(p => p.ObterPedidoPorClienteId(clienteId))
                .Returns(Task.FromResult(pedidoExistente));

            automocker.GetMock<IPedidoRepository>().Setup(p => p.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));
            
            //Act
            var response = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);
            
            //Assert
            Assert.True(response);
            automocker.GetMock<IPedidoRepository>().Verify(p => p.UnitOfWork.Commit(),Times.Once);
            automocker.GetMock<IPedidoRepository>().Verify(p => p.AtualizarPedido(pedidoExistente),Times.Once);
            automocker.GetMock<IPedidoRepository>().Verify(p => p.UnitOfWork.Rollback(),Times.Never);
            automocker.GetMock<IPedidoRepository>().Verify(p => p.AdicionarPedido(pedidoExistente),Times.Never);
        }

        [Fact]
        public async void AdicionarPedidoItemHandler_AdicionandoPedidoItemExistenteEmPedidoRascunho_DeveAtualizarOPedidoItemEPedido()
        {
            //Arrange
            var produtoId= Guid.NewGuid();
            var clienteId = Guid.NewGuid();
            var automocker = new AutoMocker();
            var pedidoHandler = automocker.CreateInstance<AdicionarPedidoItemHandler>();
            var pedidoExistente = Domain.Entities.Pedido.PedidoFactory.CriarPedidoRascunho(clienteId);
            var pedidoItemExistente = new PedidoItem(Guid.Empty, produtoId,"Samsung s10",1000,2);
            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId, produtoId,"Samsung S10", 1000, 1);

            pedidoExistente.AdicionarItem(pedidoItemExistente);

            automocker.GetMock<IPedidoRepository>().Setup(p => p.ObterPedidoPorClienteId(clienteId))
                .Returns(Task.FromResult(pedidoExistente));

            automocker.GetMock<IPedidoRepository>().Setup(p => p.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));
            
            //Act
            var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);
            
            //Assert
            Assert.True(result);
            automocker.GetMock<IPedidoRepository>().Verify(v => v.UnitOfWork.Commit(), Times.Once);
            automocker.GetMock<IPedidoRepository>().Verify(v => v.AtualizarPedido(pedidoExistente), Times.Once);
            automocker.GetMock<IPedidoRepository>().Verify(v => v.ObterPedidoPorClienteId(clienteId), Times.Once);
            automocker.GetMock<IPedidoRepository>().Verify(v => v.AtualizarPedidoItem(pedidoItemExistente), Times.Once);
        }

        [Fact]
        public async void AdicionarPedidoItemHandler_AdicionandoCommandIncorreto_DeveRetornaSuccessFalsePublicarEventos()
        {
            //Arrange
            var automocker = new AutoMocker();
            var pedidoHandler = automocker.CreateInstance<AdicionarPedidoItemHandler>();
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty,Guid.Empty, string.Empty, -100, 0);

            //Act
            var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);
            
            //Assert
            Assert.False(result);
            automocker.GetMock<IPedidoRepository>().Verify(v => v.UnitOfWork.Commit(),Times.Never);
            automocker.GetMock<IPedidoRepository>().Verify(v => v.UnitOfWork.Rollback(),Times.Never);
            automocker.GetMock<IPedidoRepository>().Verify(v => v.ObterPedidoPorClienteId(Guid.NewGuid()),Times.Never);
            automocker.GetMock<IExchangeNotification>().Verify(v => v.Publish(It.IsAny<INotification>()),Times.Exactly(5));
        }
    }
}