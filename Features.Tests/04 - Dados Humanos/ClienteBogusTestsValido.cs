using Features.Clientes;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteBogusCollection))]
    public class ClienteBogusTestsValido
    {
        private readonly ClienteFixtureBogusTests _clienteBogusFixtureTests;

        public ClienteBogusTestsValido(ClienteFixtureBogusTests clienteFixtureTests)
        {
            _clienteBogusFixtureTests = clienteFixtureTests;
        }

        [Fact(DisplayName = "Cliente Valido Bogus")]
        [Trait("Categoria","Cliente Valido Bogus")]
        public void Cliente_NovoCliente_DeveRetornaClienteValido()
        {
            //Arrange 
            Cliente cliente = _clienteBogusFixtureTests.GerarClienteValido();

            //Act
            var resultado = cliente.EhValido();

            //Assert
            Assert.Empty(cliente.ValidationResult.Errors);
            Assert.True(resultado);
        } 
    }
}
