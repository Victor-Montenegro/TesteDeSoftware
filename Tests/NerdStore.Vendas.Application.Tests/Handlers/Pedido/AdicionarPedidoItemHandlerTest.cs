using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Handlers;
using NerdStore.Vendas.Domain.Interfaces;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Handlers.Pedido
{
    public class AdicionarPedidoItemHandlerTest
    {
        [Fact]
        [Trait("", "")]
        public async void AdicionarPedidoItemHandler_NovoPedidoItem_DeveRetonarSucesso()
        {
            //Arrange
            var autoMock = new AutoMocker();
            var pedidoHandler = autoMock.CreateInstance<AdicionarPedidoItemHandler>();
            var request = new AdicionarItemPedidoRequest(Guid.NewGuid(), Guid.NewGuid(),"Samsung S10", 1000, 1);

            autoMock.GetMock<IPedidoRepository>().Setup(p => p.UnitOfWork.Commit());
            
            //Act
            var response = await pedidoHandler.Handle(request, CancellationToken.None);
            
            //Assert
            Assert.True(response.Success);
            Assert.True(response.Pedido.PedidoItem.Count == 1);
            Assert.Equal(1000, response.Pedido.ValorTotal);
            autoMock.GetMock<IPedidoRepository>().Verify(p => p.AdicionarPedido(It.IsAny<Domain.Entities.Pedido>()),Times.Once);
            // autoMock.GetMock<IMediator>().Verify(p => p.Publish(It.IsAny<INotification>(),CancellationToken.None),Times.Once);
        }
    }
}