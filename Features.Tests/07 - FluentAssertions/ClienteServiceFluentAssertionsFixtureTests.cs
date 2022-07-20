using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Features.Tests._07___FluentAssertions
{
    [CollectionDefinition(nameof(ClienteServiceFluentAssertionsFixtureTestsCollection))]
    public class ClienteServiceFluentAssertionsFixtureTestsCollection : ICollectionFixture<ClienteServiceFluentAssertionsFixtureTests > { }

    public class ClienteServiceFluentAssertionsFixtureTests : IDisposable
    {
        public AutoMocker AutoMocker { get; set; }
        public ClienteService ClienteService { get; set; }
        private IEnumerable<Cliente> GerarClientes(int qntCliente)
        {
            var gender = new Faker().PickRandom<Name.Gender>();

            var cliente = new Faker<Cliente>("pt_BR")
                                        .CustomInstantiator(f => new Cliente(
                                            Guid.NewGuid(),
                                            f.Name.FirstName(gender),
                                            f.Name.LastName(gender),
                                            f.Date.Past(80),
                                            string.Empty,
                                            f.Random.Bool(),
                                            f.Date.Past(10)
                                        ))
                                        .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Nome,c.Sobrenome));

            return cliente.Generate(qntCliente);
        }

        public Cliente GerarClienteInvalido()
        {
            Cliente cliente = GerarClientes(1).SingleOrDefault();
            DateTime dataNascimentoInvalida = new Faker().Date.Past(-17);

            cliente.SetDataNascimento(dataNascimentoInvalida);

            return cliente;
        }

        public Cliente GerarClienteValido()
        {
            Cliente cliente = GerarClientes(1).SingleOrDefault();
            DateTime dataNascimentoValida = new Faker().Date.Past(80, DateTime.Now.AddYears(-18));

            cliente.SetDataNascimento(dataNascimentoValida);

            return cliente;
        }

        public IEnumerable<Cliente> GerarClientesVariados()
            => GerarClientes(100);

        public ClienteService RetornaClienteService()
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