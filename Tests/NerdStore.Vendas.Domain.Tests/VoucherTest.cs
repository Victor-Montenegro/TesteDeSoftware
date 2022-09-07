using System;
using System.Linq;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Enums;
using NerdStore.Vendas.Domain.Validatories;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTest
    {
        [Fact(DisplayName = "Validar Voucher do Tipo Valido ")]
        [Trait("Voucher", "Vendas - Voucher")]
        public void Voucher_ValidandoVoucherCriado_DeveRetornarSemErros()
        {
            //Arrange
            var voucher = Voucher.VoucherFactory.CriarVoucherValido();
            
            //Act
            voucher.ValidarVoucher();
            
            //Assert
            Assert.True(voucher.ValidationResult.IsValid);
            Assert.Empty(voucher.ValidationResult.Errors);
        }
        
        [Fact(DisplayName = "Validar Voucher do Tipo Invalido ")]
        [Trait("Voucher", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherInvalido_DeveRetornarErros()
        {
            //Arrange
            var voucher = new Voucher(Guid.NewGuid(),
                false,
                true,
                string.Empty,
                -10,
                TipoDescontoVoucher.Valor,
                null,
                null,
                0);
            
            //Act
            voucher.ValidarVoucher();
            
            //Assert
            Assert.False(voucher.ValidationResult.IsValid);
            Assert.True(voucher.ValidationResult.Errors.Count == 6);
            Assert.Contains(VoucherValidator.AtivoErrorMsg, voucher.ValidationResult.Errors.Select(v => v.ErrorMessage));
            Assert.Contains(VoucherValidator.UtilizadoErrorMsg, voucher.ValidationResult.Errors.Select(v => v.ErrorMessage));
            Assert.Contains(VoucherValidator.DescricaoErrorMsg, voucher.ValidationResult.Errors.Select(v => v.ErrorMessage));
            Assert.Contains(VoucherValidator.QuantidadeErrorMsg, voucher.ValidationResult.Errors.Select(v => v.ErrorMessage));
            Assert.Contains(VoucherValidator.DataValidadeErrorMsg, voucher.ValidationResult.Errors.Select(v => v.ErrorMessage));
            Assert.Contains(VoucherValidator.ValorDescontoErrorMsg, voucher.ValidationResult.Errors.Select(v => v.ErrorMessage));
        }

        [Fact(DisplayName = "Validar Voucher do Tipo desconto Valor ,passando valor null")]
        [Trait("Voucher", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValorInvalido_DeveRetornarError()
        {
            //Arrange
            var voucher = new Voucher(Guid.NewGuid(),
                true,
                false,
                "20OFF",
                2,
                TipoDescontoVoucher.Valor,
                null,
                null,
                10);
            
            //Act
            voucher.ValidarVoucher();

            //Assert
            Assert.False(voucher.ValidationResult.IsValid);
            Assert.True(voucher.ValidationResult.Errors.Count == 1);
            Assert.Contains(VoucherValidator.ValorDescontoErrorMsg, voucher.ValidationResult.Errors.Select(v => v.ErrorMessage));
        }
        
        [Fact(DisplayName = "Validar Voucher do Tipo Desconto Porcentual ,passando valor null")]
        [Trait("Voucher", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoPorcentualInvalido_DeveRetornarError()
        {
            //Arrange
            var voucher = new Voucher(Guid.NewGuid(),
                true,
                false,
                "20OFF",
                2,
                TipoDescontoVoucher.Porcentual,
                null,
                null,
                10);
            
            //Act
            voucher.ValidarVoucher();

            //Assert
            Assert.False(voucher.ValidationResult.IsValid);
            Assert.True(voucher.ValidationResult.Errors.Count == 1);
            Assert.Contains(VoucherValidator.PorcentualDescontoErrorMsg, voucher.ValidationResult.Errors.Select(v => v.ErrorMessage));
        }
    }
}