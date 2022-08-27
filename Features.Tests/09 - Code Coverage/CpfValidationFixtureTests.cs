using System;
using Bogus;
using Bogus.Extensions.Brazil;
using Features.Core;
using Xunit;

namespace Features.Tests._09___Code_Coverage
{
    [CollectionDefinition(nameof(CpfValidationFixtureTestsCollection))]
    public class CpfValidationFixtureTestsCollection : ICollectionFixture<CpfValidationFixtureTests> { }

    public class CpfValidationFixtureTests : IDisposable
    {
        public string GerarCpfValido()
            => new Faker().Person.Cpf();

        // public string GerarCpfInvalido()
        //     => new Faker().Person.Cpf

        public CpfValidation RetornarCpfValidation()
            => new CpfValidation();
        
        public void Dispose()
        {
        }
    }
}