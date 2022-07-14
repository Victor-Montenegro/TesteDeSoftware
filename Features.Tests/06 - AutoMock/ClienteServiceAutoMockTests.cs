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
        private readonly ClienteService _clienteService;
        
        public ClienteServiceAutoMockTests(ClienteServiceAutoMockFixtureTests clienteServiceAutoMockFixtureTests)
        {
            _clienteServiceAutoMockFixtureTests = clienteServiceAutoMockFixtureTests;
            _clienteService = _clienteServiceAutoMockFixtureTests.ObterClienteService();
        }
        
        [Fact(DisplayName = "Tentar salvar cliente valido")]
        [Trait("Categoria ", "Cliente Service AutoMock Tests")]
        public void Adicionar_ClienteValido_DeveAdicionarCliente()
        {
            //Arrange
            Cliente cliente = _clienteServiceAutoMockFixtureTests.GerarClienteValido();
            
            //Act
            _clienteService.Adicionar(cliente);

            //Assert
            Assert.Empty(cliente.ValidationResult.Errors);
            _clienteServiceAutoMockFixtureTests.AutoMocker.GetMock<IClienteRepository>().Verify(c => c.Adicionar(cliente),Times.Once);
            _clienteServiceAutoMockFixtureTests.AutoMocker.GetMock<IMediator>().Verify(i => i.Publish(It.IsAny<INotification>(),CancellationToken.None),Times.Once);
        }

        [Fact(DisplayName = "Tentar salvar cliente invalido")]
        [Trait("Categoria ", "Cliente Service AutoMock Tests")]
        public void Adicionar_ClienteInvalido_NaoDeveAdicionarCliente()
        {
            //Arrange
            var cliente = _clienteServiceAutoMockFixtureTests.GerarClienteInvalido();
            
            //Act
            _clienteService.Adicionar(cliente);

            //Assert
            Assert.NotEmpty(cliente.ValidationResult.Errors);
            _clienteServiceAutoMockFixtureTests.AutoMocker.GetMock<IClienteRepository>().Verify(c => c.Adicionar(cliente),Times.Never);
            _clienteServiceAutoMockFixtureTests.AutoMocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None),Times.Never);
        }

        [Fact(DisplayName = "Deve buscar apenas clientes ativos ")]
        [Trait("Categoria", "Cliente Service AutoMock Tests")]
        public void ObterTodosAtivos_BuscarTodos_DeveRetornarApenasClientesAtivos()
        {
            //Arrange
            
            _clienteServiceAutoMockFixtureTests.AutoMocker.GetMock<IClienteRepository>().Setup(c => c.ObterTodos())
                .Returns(_clienteServiceAutoMockFixtureTests.ObterClientes());
            
            //Act
            List<Cliente> clientes = _clienteService.ObterTodosAtivos().ToList();
            
            //Assert
            Assert.True(clientes.Any());
            Assert.False(clientes.Count(c => !c.Ativo) > 0);
            _clienteServiceAutoMockFixtureTests.AutoMocker.GetMock<IClienteRepository>().Verify(c => c.ObterTodos(),Times.Once);
        }
    }
}