using System;
using System.Collections.Generic;
using Xunit;

namespace Demo.Tests
{
    public class FuncionarioTests
    {
        [Fact]
        public void Funcionario_Nome_NaoDeveRetornarNuloOuVazio()
        {
            //Arrange & Act
            var resultado = new Funcionario(string.Empty, 1600);

            //Assert
            Assert.False(string.IsNullOrEmpty(resultado.Nome));
        }

        [Fact]
        public void Funcionario_Apelido_NaoDeveTerApelido()
        {
            //Arrange & Act
            var resultado = new Funcionario("Douglas",2000);

            //Assert
            Assert.Null(resultado.Apelido);
            Assert.True(string.IsNullOrEmpty(resultado.Apelido));
            Assert.False(resultado.Apelido?.Length > 0);
        }

        [Fact]
        public void FuncionarioFactory_Criar_DeveRetornarNomeDoFuncionario()
        {
            //Arrange & Act
            var resultado = FuncionarioFactory.Criar("João Victor", 1600);

            //Assert
            Assert.Equal("João Victor", resultado.Nome);
        }

        [Fact]
        public void FuncionarioFactory_Criar_DeveRetornaUmTipoFuncionariO()
        {
            //Arrange & Act
            var resultado = FuncionarioFactory.Criar("Fulano",2000);

            //Assert    
            Assert.IsType<Funcionario>(resultado);
        }

        [Fact]
        public void FuncionarioFactory_Criar_DeveRetornaUmTipoPessoa()
        {
            //Arrange & Act
            var resultado = FuncionarioFactory.Criar("Fulano", 2000);

            //Assert
            Assert.IsAssignableFrom<Pessoa>(resultado);
        }

        [Fact]
        public void Funcionario_Habilidades_JuniorDeveterHabilidadesBasicas()
        {
            //Arrange & Act
            var resultado = new Funcionario("Fulano",1999);

            //Assert
            Assert.Contains("OOP",resultado.Habilidades);
        }

        [Theory]
        [InlineData(-200)]
        [InlineData(0)]
        [InlineData(499)]
        [InlineData(250)]
        [InlineData(-2000)]
        public void Funcionario_Salario_DeveRetornaExceptionPassandoSalarioInferiroPermetido(double salario)
        {
            //Arrange & Act
            var resultado = Assert.Throws<Exception>( () => new Funcionario("Fulano",salario));

            // Assert
            Assert.Equal("Salario inferior ao permitido", resultado.Message);
        }

        [Theory]
        #region InlineData
        [InlineData(500)]
        [InlineData(2000)]
        [InlineData(2001)]
        [InlineData(7999)]
        [InlineData(8000)]
        [InlineData(8001)]
        [InlineData(20000)]
        #endregion
        public void Funcionario_NivilProfissional_SalarioDeveEstaDeAcordoComONivelProfissional(double salario)
        {
            //Arrange & Act
            var resultado = new Funcionario("Gabriel", salario);

            //Assert
            if (resultado.NivelProfissional.Equals(NivelProfissional.Junior))
                Assert.InRange(resultado.Salario,500,1999);

            if (resultado.NivelProfissional.Equals(NivelProfissional.Pleno))
                Assert.InRange(resultado.Salario,2000,7999);

            if (resultado.NivelProfissional.Equals(NivelProfissional.Senior))
                Assert.InRange(resultado.Salario, 8000, double.MaxValue);

            Assert.NotInRange(resultado.Salario, double.MinValue, 499);

        }

        [Theory]
        #region InlineData
            [InlineData(NivelProfissional.Junior, new string[] { "Lógica de Programação", "OOP" }, 500)]
            [InlineData(NivelProfissional.Junior, new string[] { "Lógica de Programação", "OOP" }, 500.99)]
            [InlineData(NivelProfissional.Junior, new string[] { "Lógica de Programação", "OOP" }, 1600)]
            [InlineData(NivelProfissional.Junior, new string[] { "Lógica de Programação", "OOP" }, 1999)]
            [InlineData(NivelProfissional.Junior, new string[] { "Lógica de Programação", "OOP" }, 1999.99)]
            [InlineData(NivelProfissional.Pleno, new string[] { "Lógica de Programação", "OOP", "Testes" }, 2000)]
            [InlineData(NivelProfissional.Pleno, new string[] { "Lógica de Programação", "OOP", "Testes" }, 2000.99)]
            [InlineData(NivelProfissional.Pleno, new string[] { "Lógica de Programação", "OOP", "Testes" }, 5000)]
            [InlineData(NivelProfissional.Pleno, new string[] { "Lógica de Programação", "OOP", "Testes" }, 7999)]
            [InlineData(NivelProfissional.Pleno, new string[] { "Lógica de Programação", "OOP", "Testes" }, 7999.99)]
            [InlineData(NivelProfissional.Senior, new string[] { "Lógica de Programação", "OOP", "Testes", "Microservices" }, 8000)]
            [InlineData(NivelProfissional.Senior, new string[] { "Lógica de Programação", "OOP", "Testes", "Microservices" }, 8000.99)]
            [InlineData(NivelProfissional.Senior, new string[] { "Lógica de Programação", "OOP", "Testes", "Microservices" }, 8001)]
            [InlineData(NivelProfissional.Senior, new string[] { "Lógica de Programação", "OOP", "Testes", "Microservices" }, 15000.99)]
            [InlineData(NivelProfissional.Senior, new string[] { "Lógica de Programação", "OOP", "Testes", "Microservices" }, 30000)]
        #endregion
        public void FuncionarioFactory_Criar_DeveRetornarNivelProfissionalEHabilidadesCorrespondenteAoSalario(NivelProfissional nivelEsperado, IList<string> habilidadesEsperadas, double salario)
        {

            //Arrange & Act
            var resultado = FuncionarioFactory.Criar(string.Empty, salario);

            //Assert
            Assert.Equal(nivelEsperado, resultado.NivelProfissional);
            Assert.Equal(habilidadesEsperadas,resultado.Habilidades);
        }

        [Theory]
        #region InlineData
        [InlineData(NivelProfissional.Junior, new string[] { "Lógica de Programação", "OOP" },  3500, 1999.99)]
        [InlineData(NivelProfissional.Junior, new string[] { "Lógica de Programação", "OOP" }, 1000, 1999)]
        [InlineData(NivelProfissional.Junior, new string[] { "Lógica de Programação", "OOP" }, 500, 1600)]
        [InlineData(NivelProfissional.Junior, new string[] { "Lógica de Programação", "OOP" }, 10000, 800)]
        [InlineData(NivelProfissional.Pleno, new string[] { "Lógica de Programação", "OOP", "Testes" }, 500.99, 2000)]
        [InlineData(NivelProfissional.Pleno, new string[] { "Lógica de Programação", "OOP", "Testes" }, 1999.99, 2000)]
        [InlineData(NivelProfissional.Pleno, new string[] { "Lógica de Programação", "OOP", "Testes" }, 1100.99, 2000.99)]
        [InlineData(NivelProfissional.Pleno, new string[] { "Lógica de Programação", "OOP", "Testes" }, 8000, 5000)]
        [InlineData(NivelProfissional.Pleno, new string[] { "Lógica de Programação", "OOP", "Testes" }, 4000, 7600)]
        [InlineData(NivelProfissional.Senior, new string[] { "Lógica de Programação", "OOP", "Testes", "Microservices" }, 500, 8000)]
        [InlineData(NivelProfissional.Senior, new string[] { "Lógica de Programação", "OOP", "Testes", "Microservices" }, 10000, 8000)]
        [InlineData(NivelProfissional.Senior, new string[] { "Lógica de Programação", "OOP", "Testes", "Microservices" }, 5000, 8000)]
        #endregion
        public void FuncionarioFactory_DefinirSalario_DeveRetornarNivelProfissionalEHabilidadesCorrespondeAoSalario(NivelProfissional nivelEsperado,IList<string> habilidadesEsperadas, double salarioAnterior, double novoSalario)
        {
            //Arrange
            Funcionario funcionario = FuncionarioFactory.Criar(string.Empty,salarioAnterior);
            
            //Act
            funcionario.DefinirSalario(novoSalario);

            //Assert
            Assert.Equal(nivelEsperado,funcionario.NivelProfissional);
            Assert.Equal(habilidadesEsperadas, funcionario.Habilidades);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(2000)]
        [InlineData(5000)]
        [InlineData(10000)]
        [InlineData(2400)]
        public void Funcionario_Habilidades_HabilidadesNaoDevemRetornarVazio(double salario)
        {
            //Arrange & Act
            var resultado = new Funcionario("Fulano",salario);

            //Assert
            Assert.All(resultado.Habilidades, Habilidade => Assert.False(string.IsNullOrEmpty(Habilidade)));
        }
    }
}
