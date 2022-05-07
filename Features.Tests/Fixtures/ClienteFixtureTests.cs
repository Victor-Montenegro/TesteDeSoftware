using Features.Clientes;
using System;
using Xunit;

namespace Features.Tests.Fixtures
{
    [Collection(nameof(ClienteCollection))]
    public class ClienteCollection : ICollectionFixture<ClienteFixtureTests> { }    

    public class ClienteFixtureTests : IDisposable
    {
        public Cliente ClienteValido()
        {
            Cliente cliente = new Cliente(Guid.NewGuid(), "João Victor", "Montenegro Rocha", DateTime.Now.AddYears(-30), "jvcmontenegro67@gmail.com", true, DateTime.Now);

            return cliente;
        }

        public Cliente ClienteInvalido()
        {
            Cliente cliente = new Cliente(Guid.NewGuid(), "", "", DateTime.Now.AddYears(-30), "jvcmontenegro67@gmail.com", true, DateTime.Now);

            return cliente;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
