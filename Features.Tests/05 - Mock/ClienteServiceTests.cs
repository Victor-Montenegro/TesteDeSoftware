using System.Linq;
using System.Threading;
using Features.Clientes;
using MediatR;
using Moq;
using Xunit;

namespace Features.Tests._05___Mock
{
    [Collection(nameof(ClienteServiceFixtureTestsColletion))]
    public class ClienteServiceTests
    {
        private ClienteServiceFixtureTests _clienteServiceFixtureTests;

        public ClienteServiceTests(ClienteServiceFixtureTests clienteServiceFixtureTests)
        {
            _clienteServiceFixtureTests = clienteServiceFixtureTests;
        }

        [Fact(DisplayName = "Adicionar cliente com sucesso")]
        [Trait("Categoria", "Client Service Mock Test")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            //Arrange 
            var imediator = new Mock<IMediator>();
            var clienteRepository = new Mock<IClienteRepository>();
            Cliente cliente = _clienteServiceFixtureTests.GerarClienteValido();
            ClienteService clienteService = new ClienteService(clienteRepository.Object, imediator.Object);

            //Act
            clienteService.Adicionar(cliente);

            //Assert
            Assert.True(cliente.EhValido());
            clienteRepository.Verify(r => r.Adicionar(cliente), Times.Once);
            imediator.Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Tentar adicionar cliente invalido")]
        [Trait("Categoria","Client Service Mock Test")]
        public void ClienteService_Adicionar_DeveFalharQuandoClienteEstaInvalido()
        {
            //Arrange
            Cliente cliente = _clienteServiceFixtureTests.GerarClienteInvalido();

            var clienteRepository = new Mock<IClienteRepository>();
            var mediator = new Mock<IMediator>();

            ClienteService clienteService = new ClienteService(clienteRepository.Object, mediator.Object);
            
            //Act
            clienteService.Adicionar(cliente);
            
            //Assert
            Assert.False(cliente.EhValido());
            clienteRepository.Verify(cr => cr.Adicionar(cliente),Times.Never);
            mediator.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Tentar buscar todos os clientes ativos")]
        [Trait("Categoria", "Client Service Mock Test")]
        public void ClienteService_BuscarTodos_DeveRetornarApenasClientesAtivos()
        {
            //Arrange 
            var mediator = new Mock<IMediator>();
            var clienteRepository = new Mock<IClienteRepository>();
            ClienteService clienteService = new ClienteService(clienteRepository.Object, mediator.Object);
   
            clienteRepository.Setup(s => s.ObterTodos())
                .Returns(_clienteServiceFixtureTests.GerarClientesVariados());
            
            //Act
            var cliente = clienteService.ObterTodosAtivos();

            //Assert
            Assert.True(cliente.Any());
            Assert.False(cliente.Count(c => !c.Ativo) > 0);
            clienteRepository.Verify(c => c.ObterTodos(),Times.Once);
        }
    }
}