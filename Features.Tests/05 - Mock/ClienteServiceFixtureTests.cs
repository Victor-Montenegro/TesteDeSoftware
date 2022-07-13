using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using Xunit;

namespace Features.Tests._05___Mock
{
    [CollectionDefinition(nameof(ClienteServiceFixtureTestsColletion))]
    public class ClienteServiceFixtureTestsColletion : ICollectionFixture<ClienteServiceFixtureTests>
    {
    }

    public class ClienteServiceFixtureTests : IDisposable
    {
        private IEnumerable<Cliente> GerarClientes(int quantidadeClienteGerados)
        {
            var gender = new Faker().PickRandom<Name.Gender>();

            var cliente = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(),
                    f.Name.FirstName(gender),
                    f.Name.LastName(gender),
                    f.Date.Past(80),
                    string.Empty,
                    true,
                    f.Date.Past(10)
                ))
                .RuleFor(c => c.Email,(f,c) => f.Internet.Email(c.Nome,c.Sobrenome));

            return cliente.Generate(quantidadeClienteGerados);
        }

        public Cliente GerarClienteValido()
        {
            Cliente cliente = GerarClientes(1).SingleOrDefault();

            DateTime dataNascimentoValida = new Faker().Date.Past(80, DateTime.Now.AddYears(-18));

            cliente.SetDataNascimento(dataNascimentoValida);

            return cliente;
        }
        
        public Cliente GerarClienteInvalido()
        {
            Cliente cliente = GerarClientes(1).SingleOrDefault();

            DateTime dataNascimentoValida = new Faker().Date.Past(17);

            cliente.SetDataNascimento(dataNascimentoValida);

            return cliente;
        }

        public IEnumerable<Cliente> GerarClientesVariados()
            => GerarClientes(100);
        
        public void Dispose()
        {
        }
    }
}