using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteBogusCollection))]
    public class ClienteBogusCollection : ICollectionFixture<ClienteFixtureBogusTests>{  }

    public class ClienteFixtureBogusTests : IDisposable
    {
        public IEnumerable<Cliente> GerarClientes(int quantidade, bool? ativo)
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var cliente = new Faker<Cliente>()
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(),
                    nome: f.Name.FirstName(genero),
                    sobrenome: f.Name.LastName(genero),
                    dataNascimento: f.Date.Past(80, DateTime.Now.AddYears(-4)),
                    email: string.Empty,
                    ativo: ativo ?? f.Random.Bool(),
                    dataCadastro: DateTime.Now
                ))
                .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Nome, c.Sobrenome));

            return cliente.Generate(quantidade);
        }

        public Cliente GerarClienteValido()
        {
            Cliente cliente = GerarClientes(1, null).FirstOrDefault();
            DateTime dataNascimento = new Faker().Date.Past(80, DateTime.Now.AddYears(-18));

            cliente.SetDataNascimento(dataNascimento);

            return cliente;
        }

        public Cliente GerarClienteInvalido()
        {
            Cliente cliente = GerarClientes(1, null).FirstOrDefault();
            var dataNascimento = new Faker().Date.Past(-4);

            cliente.SetDataNascimento(dataNascimento);

            return cliente;
        }

        public IEnumerable<Cliente> GerarClientesVariados()
            => GerarClientes(100, null);
        
        // public Cliente GerarClienteValido()
        // {
        //     var genero = new Faker().PickRandom<Name.Gender>();
        //
        //     Cliente clienteFake = new Faker<Cliente>("pt_BR")
        //         .CustomInstantiator(f => new Cliente(
        //                 Guid.NewGuid(),
        //                 f.Name.FirstName(genero),
        //                 f.Name.LastName(genero),
        //                 f.Date.Past(80, DateTime.Now.AddYears(-18)),
        //                 string.Empty,
        //                 true,
        //                 f.Date.Past(10)
        //             ))
        //         .RuleFor(c => c.Email, (f,c) => f.Internet.Email(c.Nome,c.Sobrenome));
        //
        //     return clienteFake;
        // }
        //
        // public Cliente GerarClienteInvalido()
        // {
        //     var genero = new Faker().PickRandom<Name.Gender>();
        //
        //     Cliente cliente = new Faker<Cliente>("pt_BR")
        //         .CustomInstantiator(f => new Cliente(
        //                     Guid.NewGuid(),
        //                     f.Name.FirstName(),
        //                     f.Name.LastName(),
        //                     f.Date.Past(17,DateTime.Now.AddYears(-1)),
        //                     String.Empty,
        //                     false,
        //                     DateTime.Now
        //                     ));
        //
        //     return cliente;
        // }

        public void Dispose()
        {
        }
    }
}