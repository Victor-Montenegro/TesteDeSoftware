using Xunit;
using Demo;

namespace Demo.Tests
{
    public class CalculadoraTests
    {
        [Fact]
        public void Calculadora_Somar_RetornaResultadoSoma()
        {
            //Arrange
            Calculadora calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Somar(2,2);

            //Assert
            Assert.Equal(4,resultado);
        }

        [Fact]
        public void Calculadora_Dividir_RetornarResultadoDivisao()
        {
            //Arrange
            Calculadora calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Dividir(4,2);

            //Assert
            Assert.Equal(2,resultado);
        }

        [Fact] 
        public void Calculadora_Somar_NaoRetornarValorErrado()
        {
            //Arrange
            Calculadora calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Somar(1.13123123123,2.2312313123);
            
            //Assert
            Assert.NotEqual(3.3,resultado, 1);
        }

        [Theory]
        [InlineData(1,1,2)]
        [InlineData(3.1,3.1,6.2)]
        [InlineData(20,20,40)]
        [InlineData(0,20,20)]
        public void Calculadora_Somar_RetornaValoresSomados(double v1, double v2, double total)
        {
            //Arrange
            Calculadora calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Somar(v1,v2);

            //Assert
            Assert.Equal(total,resultado);
        }

        [Theory]
        [InlineData(1,1,1)]
        [InlineData(120,3,40)]
        [InlineData(7,2,3.5)]
        [InlineData(3,2,1.5)]
        public void Calculadora_Dividir_RetornarValoresDivididos(double valor1, double valor2, double total)
        {
            //Arrange
            Calculadora calculadora = new Calculadora();

            //Act
            var resultado = calculadora.Dividir(valor1,valor2);

            //Assert
            Assert.Equal(total,resultado);
        }
    }
}
