using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using System;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteBogusCollection))]
    public class ClienteBogusCollection : ICollectionFixture<ClienteFixtureBogusTests> { }

    public class ClienteFixtureBogusTests : IDisposable
    {
        public Cliente GerarClienteValido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            Cliente clienteFake = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                        Guid.NewGuid(),
                        f.Name.FirstName(genero),
                        f.Name.LastName(genero),
                        f.Date.Past(80, DateTime.Now.AddYears(-18)),
                        string.Empty,
                        true,
                        f.Date.Past(10)
                    ))
                .RuleFor(c => c.Email, (f,c) => f.Internet.Email(c.Nome,c.Sobrenome));

            return clienteFake;
        }

        public void Dispose()
        {
        }
    }
}
