using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using Moq.AutoMock;
using Xunit;

namespace Features.Tests._06___AutoMock
{
    [CollectionDefinition(nameof(ClienteServiceAutoMockFixtureTestsColletion))]
    public class ClienteServiceAutoMockFixtureTestsColletion : ICollectionFixture<ClienteServiceAutoMockFixtureTests> { }

    public class ClienteServiceAutoMockFixtureTests : IDisposable
    {
        public ClienteService ClienteService;
        public AutoMocker AutoMocker;
        
        private ICollection<Cliente> GerarCliente(int qnt)
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var cliente = new Faker<Cliente>()
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(),
                    nome: f.Name.FirstName(genero),
                    sobrenome: f.Name.LastName(genero),
                    dataNascimento: f.Date.Past(80),
                    email: String.Empty,
                    ativo: f.Random.Bool(),
                    dataCadastro: f.Date.Past(10)
                ))
                .RuleFor(c => c.Email,(f,c) => f.Internet.Email(c.Nome,c.Sobrenome));

            return cliente.Generate(qnt);
        }

        public Cliente GerarClienteValido()
        {
            Cliente cliente = GerarCliente(1).FirstOrDefault();

            DateTime dataNascValida = new Faker().Date.Past(80, DateTime.Now.AddYears(-18));

            cliente.SetDataNascimento(dataNascValida);

            return cliente;
        }

        public Cliente GerarClienteInvalido()
        {
            Cliente cliente = GerarCliente(1).FirstOrDefault();

            DateTime dataNascInvalido = new Faker().Date.Past(17);

            cliente.SetDataNascimento(dataNascInvalido);

            return cliente;
        }

        public IEnumerable<Cliente> ObterClientes()
            => GerarCliente(100);

        public ClienteService ObterClienteService()
        {
            AutoMocker = new();
            ClienteService = AutoMocker.CreateInstance<ClienteService>();

            return ClienteService;
        }

        public void Dispose()
        {
        }
    }
}