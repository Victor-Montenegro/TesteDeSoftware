using System.ComponentModel.Design;
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
        
        [Fact(DisplayName = "Tentar Buscar todos os clientes ativos")]
        [Trait("Categoria ", "Cliente Service AutoMock Tests")]
        public void Adicionar_ClienteValido_DeveAdicionarCliente()
        {
            //Arrange
            Cliente cliente = _clienteServiceAutoMockFixtureTests.GerarClienteValido();
            var clienteRepository = new Mock<IClienteRepository>();
            var imediator = new Mock<IMediator>();
            ClienteService clienteService = new ClienteService(clienteRepository.Object, imediator.Object);
            
            //Act
            clienteService.Adicionar(cliente);

            //Assert
            Assert.Empty(cliente.ValidationResult.Errors);
            clienteRepository.Verify(c => c.Adicionar(cliente),Times.Once);
            imediator.Verify(i => i.Publish(It.IsAny<INotification>(),CancellationToken.None),Times.Once);
        }

        [Fact(DisplayName = "Tentar Buscar todos os clientes invalidos")]
        [Trait("Categoria ", "Cliente Service AutoMock Tests")]
        public void Adicionar_ClienteInvalido_NaoDeveAdicionarCliente()
        {
            //Arrange

            //Act
            
            //Assert
        }
    }
}