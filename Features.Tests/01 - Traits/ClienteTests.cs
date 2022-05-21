using Features.Clientes;
using System;
using Xunit;

namespace Features.Tests.Traits
{
    public class ClienteTests
    {
        [Fact(DisplayName = "Novo Cliente")]
        [Trait("Cliente","Cenario Novo Cliente")]
        public void Cliente_NovoCliente_ClienteDeveEstaValido()
        {
            //Arrange
            Cliente cliente = new Cliente(Guid.NewGuid(),"João Victor","Montenegro Rocha",DateTime.Now.AddYears(-23),"jvcmontenegro67@gmail.com",true,DateTime.Now);

            //Act
            var resultado = cliente.EhValido();

            //Assert
            Assert.True(resultado);
            Assert.Empty(cliente.ValidationResult.Errors);
        }

        [Theory(DisplayName = "Novo Cliente")]
        [Trait("Cliente", "Cenario Novo Cliente")]
        [InlineData("J","Montenegro Rocha",-23,"jvc@jvc.com",true)]
        [InlineData("João Victor","M",-23,"jvc@jvc.com",true)]
        [InlineData("João Victor", "Montenegro Rocha", -10, "jvc@jvc.com", true)]
        [InlineData("João Victor", "Montenegro Rocha", -23, "jvcjvc.com", true)]
        [InlineData("J", "M", -3, "vc.com", true)]
        public void Cliente_NovoCliente_ClienteDeveEstaInvalido(string nome, string sobrenome, int nascimento, string email, bool ativo)
        {
            //Arrange
            Cliente cliente = new Cliente(Guid.NewGuid(),nome,sobrenome,DateTime.Now.AddYears(nascimento),email,ativo,DateTime.Now);

            //Act
            var resultado = cliente.EhValido();

            //Assert
            Assert.False(resultado);
            Assert.NotEmpty(cliente.ValidationResult.Errors);
        }










    }
}
