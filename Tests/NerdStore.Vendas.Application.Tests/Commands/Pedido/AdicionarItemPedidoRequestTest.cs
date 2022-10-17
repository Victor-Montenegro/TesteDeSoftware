using System;
using System.Linq;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain.Helpers;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedido
{
    public class AdicionarItemPedidoRequestTest
    {
        [Fact]
        [Trait("", "")]
        public void AdicionarItemPedidoRequest_AdicionarRequestValido_NaoDeveRetornarError()
        {
            //Arrange
            var request = new AdicionarItemPedidoCommand(Guid.NewGuid(),Guid.NewGuid(), "Teste request",100,2);

            //Act
            request.EhValido();

            //Assert
            Assert.True(request.ValidationResult.IsValid);
            Assert.Empty(request.ValidationResult.Errors);
        }

        [Fact]
        [Trait("", "")]
        public void AdicionarItemPedidoRequest_AdicionarRequestInvalido_DeveRetornarError()
        {
            //Arrange
            var quantidadeErrosEsperados = 5;
            var request = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, string.Empty, PedidoItemHelper.MIN_UNIDADE_PRODUTO - 1, 0);

            //Act
            request.EhValido();

            //Assert
            Assert.False(request.ValidationResult.IsValid);
            Assert.NotEmpty(request.ValidationResult.Errors);
            Assert.Equal(5,request.ValidationResult.Errors.Count);
            Assert.Contains(AdicionarItemPedidoValidator.NomeErrorMsg, request.ValidationResult.Errors.Select(s => s.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidator.ValorErrorMsg, request.ValidationResult.Errors.Select(s => s.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidator.IdClienteErrorMsg, request.ValidationResult.Errors.Select(s => s.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidator.IdProdutoErrorMsg, request.ValidationResult.Errors.Select(s => s.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidator.QuantidadeMinErrorMsg, request.ValidationResult.Errors.Select(s => s.ErrorMessage));

        }

        [Fact]
        [Trait("", "")]
        public void AdicionarItemPedidoRequest_AdicionarItemComQuantidadeAcimaPermitido_DeveRetornarError()
        {
            //Arrange
            var request = new AdicionarItemPedidoCommand(Guid.NewGuid(),Guid.NewGuid(), "Teste request",100,PedidoItemHelper.MAX_UNIDADE_PRODUTO + 1);
            //Act
            request.EhValido();
            
            //Assert
            Assert.False(request.ValidationResult.IsValid);
            Assert.NotEmpty(request.ValidationResult.Errors);
            Assert.Equal(1,request.ValidationResult.Errors.Count);
            Assert.Contains(AdicionarItemPedidoValidator.QuantidadeMaxErrorMsg,request.ValidationResult.Errors.Select(s => s.ErrorMessage));
        }
    }
}