using System.Threading;
using Features.Clientes;
using FluentAssertions;
using FluentAssertions.Extensions;
using MediatR;
using Moq;
using Xunit;

namespace Features.Tests._07___FluentAssertions
{
    [Collection(nameof(ClienteServiceFluentAssertionsFixtureTestsCollection))]
    public class ClienteServiceFluentAssertionsTests
    {
        private ClienteServiceFluentAssertionsFixtureTests _assertionsFixture;
        private ClienteService _clienteService;

        public ClienteServiceFluentAssertionsTests(ClienteServiceFluentAssertionsFixtureTests assertionsFixture)
        {
            _assertionsFixture = assertionsFixture;
            _clienteService = _assertionsFixture.RetornaClienteService();
        }

        [Fact(DisplayName = "Deve salvar o cliente valido")]
        [Trait("Categoria", "Fluent Assertions ClienteService Tests")]
        public void ClienteService_Adicionar_DeveAdicionarCorretamente()
        {
            //Arrange
            Cliente cliente = _assertionsFixture.GerarClienteValido();

            //Act
            _clienteService.Adicionar(cliente);

            //Assert
            cliente.ValidationResult.Errors.Should().HaveCount(0);
            _assertionsFixture.AutoMocker.GetMock<IClienteRepository>()
                .Verify(c => c.Adicionar(cliente), Times.Once);
            
            _assertionsFixture.AutoMocker.GetMock<IMediator>()
                .Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once());
        }

        [Fact(DisplayName = "Nao deve salvar cliente invalido")]
        [Trait("Categoria", "Fluent Assertions ClienteService Tests")]
        public void ClienteService_Adicionar_ClienteInvalidoDeveRetornaError()
        {
            //Arrange
            Cliente cliente = _assertionsFixture.GerarClienteInvalido();

            //Act
            _clienteService.Adicionar(cliente);

            //Assert
            cliente.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1);
            _assertionsFixture.AutoMocker.GetMock<IClienteRepository>()
                .Verify(c => c.Adicionar(cliente), Times.Never);
            
            _assertionsFixture.AutoMocker.GetMock<IMediator>()
                .Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Retornar apenas clientes ativos")]
        [Trait("Categoria", "Fluent Assertions ClienteService Tests")]
        public void ClienteService_ObterTodosAtivos_DeveRetornaApenasClientesAtivos()
        {
            //Arrange
            _assertionsFixture.AutoMocker.GetMock<IClienteRepository>()
                .Setup(c => c.ObterTodos())
                .Returns(_assertionsFixture.GerarClientesVariados());

            //Act
            var clientes = _clienteService.ObterTodosAtivos();
            
            //Assert
            clientes.Should()
                .HaveCountGreaterThan(0)
                .And
                .OnlyHaveUniqueItems();

            clientes.Should()
                .NotContain(n => !n.Ativo,"Todos os clientes devem ser ativos");

            _assertionsFixture.AutoMocker.GetMock<IClienteRepository>()
                .Verify(c => c.ObterTodos(),Times.Once);

            //APENAS UMA DEMOSTRAÇÂO, MAS ESSE TIPO DE VALIDACAO DEVE SER FEITA NO CONTEXTO DE TESTE DE INTEGRACAO
            _clienteService.ExecutionTimeOf(c => c.ObterTodosAtivos())
                .Should()
                .BeLessThan(50.Milliseconds());
        }
    }
}