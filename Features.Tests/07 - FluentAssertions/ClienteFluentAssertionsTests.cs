using Features.Clientes;
using FluentAssertions;
using Xunit;

namespace Features.Tests._07___FluentAssertions
{
    [Collection(nameof(ClienteServiceFluentAssertionsFixtureTestsCollection))]
    public class ClienteFluentAssertionsTests
    {
        private ClienteServiceFluentAssertionsFixtureTests _assertionsFixture;
        
        public ClienteFluentAssertionsTests(ClienteServiceFluentAssertionsFixtureTests assertionsFixture)
        {
            _assertionsFixture = assertionsFixture;
        }

        [Fact(DisplayName = "O cliente criado deve ser valido")]
        [Trait("Categoria", "Fluent Assertions Cliente Tests")]
        public void Cliente_NovoCliente_ClienteDeveSerValido()
        {
            //Arrange
            Cliente cliente = _assertionsFixture.GerarClienteValido();
            
            //Act
            var result = cliente.EhValido();
            
            //Assert
            result.Should().BeTrue();
            cliente.ValidationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = "O Cliente criado deve ser invalido")]
        public void Cliente_NovoCliente_ClienteDeveSerInvalido()
        {
            //Arrange
            Cliente cliente = _assertionsFixture.GerarClienteInvalido();
            
            //Act
            var result = cliente.EhValido();
            
            //Assert
            result.Should().BeFalse();
            cliente.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1);
        }
    }
}