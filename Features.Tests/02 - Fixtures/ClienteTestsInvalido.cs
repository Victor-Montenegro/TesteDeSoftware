using Features.Clientes;
using Xunit;

namespace Features.Tests.Fixtures
{
    [Collection(nameof(ClienteCollection))]
    public class ClienteTestsInvalido
    {
        private readonly ClienteFixtureTests _clienteFixtureTests;

        public ClienteTestsInvalido(ClienteFixtureTests clienteFixtureTests)
        {
            _clienteFixtureTests = clienteFixtureTests;
        }

        [Fact]
        public void Cliente_NovoCLiente_DeveRetornaClienteInvalido()
        {
            //Arrange
            Cliente cliente = _clienteFixtureTests.ClienteInvalido();

            //Act
            var resultado = cliente.EhValido();

            //Assert
            Assert.NotEmpty(cliente.ValidationResult.Errors);
            Assert.False(resultado);
        }
    }
}
