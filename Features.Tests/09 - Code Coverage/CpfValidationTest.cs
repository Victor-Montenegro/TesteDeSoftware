using Features.Core;
using FluentAssertions;
using Xunit;

namespace Features.Tests._09___Code_Coverage
{
    [Collection(nameof(CpfValidationFixtureTestsCollection))]
    public class CpfValidationTest
    {
        private readonly CpfValidationFixtureTests _cpfValidationFixtureTests;

        private readonly CpfValidation _cpfValidation;

        public CpfValidationTest(CpfValidationFixtureTests cpfValidationFixtureTests)
        {
            _cpfValidationFixtureTests = cpfValidationFixtureTests;
            _cpfValidation = _cpfValidationFixtureTests.RetornarCpfValidation();
        }
        
        
        // [Fact(DisplayName = "Realizar validação de cpf invalido")]
        // [Trait("Categoria", "Code Coverage CpfValidation Test")]
        // public void CpfValidation_Adicionar_ValidandoCpfInvalido()
        // {
        //     //Arrange
        //     string cpfInvalido = _cpfValidationFixtureTests.GerarCpfValido();
        //     //Act
        //
        //     //Assert
        // }
        
        [Fact(DisplayName = "Realizar validação de cpf valido")]
        [Trait("Categoria", "Code Coverage CpfValidation Test")]
        public void CpfValidation_Validando_AdicionandoCpfValido()
        {
            //Arrange
            string cpfInvalido = _cpfValidationFixtureTests.GerarCpfValido();
            
            //Act
            bool result = _cpfValidation.EhValido(cpfInvalido);
            
            //Assert
            result.Should().BeTrue();
        }
    }
}