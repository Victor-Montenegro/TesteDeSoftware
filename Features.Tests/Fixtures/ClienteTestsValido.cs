using Features.Clientes;
using Xunit;

namespace Features.Tests.Fixtures
{
    [Collection(nameof(ClienteCollection))]
    public class ClienteTestsValido
    {
        private readonly ClienteFixtureTests _clienteFixtureTests;

        public ClienteTestsValido(ClienteFixtureTests clienteFixtureTests)
        {
            _clienteFixtureTests = clienteFixtureTests;
        }

        [Fact]
        public void Cliente_NovoCliente_DeveRetornaClienteValido()
        {
            //Arrange 
            Cliente cliente = _clienteFixtureTests.ClienteValido();

            //Act
            var resultado = cliente.EhValido();

            //Assert
            Assert.Empty(cliente.ValidationResult.Errors);
            Assert.True(resultado);
        } 
    }
}
