using Xunit;

namespace Demo.Tests
{
    public class StringsToolsTests
    {
        [Fact]
        public void StringsTools_Unir_RetornarValorUnido()
        {
            //Arrange
            StringsTools stringsTools = new StringsTools();

            //Act
            var resultado = stringsTools.Unir("João","Victor");

            //Assert
            Assert.Equal("João Victor",resultado);
        }

        [Fact]
        public void StringsTools_Unir_RetornarValorUnidoIgonorandoMaiusculoMinusculo()
        {
            //Arrange
            StringsTools stringsTools = new StringsTools();

            //Act
            var resultado = stringsTools.Unir("ValmiR", "FeRREira");

            //Assert
            Assert.Equal("VALMIR FERREIRA",resultado,true);
        }

        [Fact]
        public void StringsTools_Unir_ConterOValorEsperadoNoPrefixoDoValorRetornado()
        {
            //Arrange
            StringsTools stringsTools = new StringsTools();

            //Act
            var resultado = stringsTools.Unir("João","Victor");

            //Assert
            Assert.StartsWith("Jo",resultado);
        }

        [Fact]
        public void StringsTools_Unir_ConterValorEsperadoNoPosfixoDoValorRetornado()
        {
            //Arrange
            StringsTools stringsTools = new StringsTools();

            //Act
            var resultado = stringsTools.Unir("Franci","Montenegro");

            //Assert
            Assert.EndsWith("negro",resultado);
        }

        [Fact]
        public void StringsTools_Unir_NaoConterNumeracaoNoValorRetornado()
        {
            //Arrange
            StringsTools stringsTools= new StringsTools();

            //Act
            var resultado = stringsTools.Unir("Radeon","RX");

            //Assert
            Assert.DoesNotMatch(@"[0-9]",resultado);
        }

        [Fact]
        public void StringsTools_Unir_ConterLetraMaisculaParaCadaPalavraDoValorRetornado()
        {
            //Arrange
            StringsTools stringsTools = new StringsTools();

            //Act
            var resultado = stringsTools.Unir("Kraken","World");

            //Assert
            Assert.Matches(@"[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", resultado);
        }

        [Theory]
        [InlineData("Francisco", "Antonio","Francisco Antonio")]
        [InlineData("Wagner","Cabral","Wagner Cabral")]
        [InlineData("João","Batista","João Batista")]
        [InlineData("Montenegro","Rocha","Montenegro Rocha")]
        public void StringsTools_Unir_RetornarValoresUnidos(string valor1, string valor2, string valorEsperado)
        {
            //Arrange
            StringsTools stringsTools = new StringsTools();

            //Act
            var resultado = stringsTools.Unir(valor1,valor2);

            //Assert
            Assert.Equal(valorEsperado, resultado);
        }

        [Theory]
        [InlineData("Valeria", "Santos", "Vale")]
        [InlineData("Barbara", "Barbaria", "Barba")]
        [InlineData("Carlos", "Dama", "Car")]
        [InlineData("Junior", "Juniores", "Juni")]
        public void StringsTools_Unir_ConterValorEsperadoNoPrefixoDosValoresRetornados(string valor1, string valor2, string valorEsperado)
        {
            //Arrange
            StringsTools stringsTools = new StringsTools();

            //Act
            var resultado = stringsTools.Unir(valor1,valor2);

            //Assert
            Assert.StartsWith(valorEsperado,resultado);
        }


        [Theory]
        [InlineData("Valeria", "Dolores", "lores")]
        [InlineData("Savio", "Ferreira", "reira")]
        [InlineData("Damasco", "Damasceno", "sceno")]
        [InlineData("Jim", "Willson", "son")]
        public void StringsTools_Unir_ConterValorEsperadoNoPosfixoDosValoresRetornados(string valor1, string valor2, string valorEsperado)
        {
            //Arrange
            StringsTools stringsTools = new StringsTools();

            //Act
            var resultado = stringsTools.Unir(valor1,valor2);

            //Assert
            Assert.EndsWith(valorEsperado,resultado);
        }

    }
}
