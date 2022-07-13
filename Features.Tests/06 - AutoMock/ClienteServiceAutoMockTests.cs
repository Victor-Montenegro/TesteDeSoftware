using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Features.Clientes;
using MediatR;
using Moq;
using Xunit;

namespace Features.Tests._06___AutoMock
{
    [Collection(nameof(ClienteServiceAutoMockFixtureTestsColletion))]
    public class ClienteServiceAutoMockTests
    {
        private ClienteServiceAutoMockFixtureTests _clienteServiceAutoMockFixtureTests;

        public ClienteServiceAutoMockTests(ClienteServiceAutoMockFixtureTests clienteServiceAutoMockFixtureTests)
        {
            _clienteServiceAutoMockFixtureTests = clienteServiceAutoMockFixtureTests;
        }
        
        [Fact(DisplayName = "Tentar salvar cliente valido")]
        [Trait("Categoria ", "Cliente Service AutoMock Tests")]
        public void Adicionar_ClienteValido_DeveAdicionarCliente()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            var clienteRepository = new Mock<IClienteRepository>();
            Cliente cliente = _clienteServiceAutoMockFixtureTests.GerarClienteValido();
            ClienteService clienteService = new ClienteService(clienteRepository.Object, mediator.Object);
            
            //Act
            clienteService.Adicionar(cliente);

            //Assert
            Assert.Empty(cliente.ValidationResult.Errors);
            clienteRepository.Verify(c => c.Adicionar(cliente),Times.Once);
            mediator.Verify(i => i.Publish(It.IsAny<INotification>(),CancellationToken.None),Times.Once);
        }

        [Fact(DisplayName = "Tentar salvar cliente invalido")]
        [Trait("Categoria ", "Cliente Service AutoMock Tests")]
        public void Adicionar_ClienteInvalido_NaoDeveAdicionarCliente()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            var clienteRepository = new Mock<IClienteRepository>();
            Cliente cliente = _clienteServiceAutoMockFixtureTests.GerarClienteInvalido();
            ClienteService clienteService = new ClienteService(clienteRepository.Object, mediator.Object);
            
            //Act
            clienteService.Adicionar(cliente);

            //Assert
            Assert.NotEmpty(cliente.ValidationResult.Errors);
            clienteRepository.Verify(c => c.Adicionar(cliente),Times.Never);
            mediator.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None),Times.Never);
        }

        [Fact(DisplayName = "Deve buscar apenas clientes ativos ")]
        [Trait("Categoria", "Cliente Service AutoMock Tests")]
        public void ObterTodosAtivos_BuscarTodos_DeveRetornarApenasClientesAtivos()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            var clienteRepository = new Mock<IClienteRepository>();
            ClienteService clienteService = new ClienteService(clienteRepository.Object, mediator.Object);
            clienteRepository.Setup(c => c.ObterTodos())
                .Returns(_clienteServiceAutoMockFixtureTests.ObterClientes());
            
            //Act
            List<Cliente> clientes = clienteService.ObterTodosAtivos().ToList();
            
            //Assert
            Assert.True(clientes.Any());
            Assert.False(clientes.Count(c => !c.Ativo) > 0);
            clienteRepository.Verify(c => c.ObterTodos(),Times.Once);
        }
    }
}