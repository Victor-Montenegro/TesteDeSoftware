using System;
using FluentValidation;
using NerdStore.Vendas.Domain.Entities;
using NerdStore.Vendas.Domain.Enums;

namespace NerdStore.Vendas.Domain.Validatories
{
    public class VoucherValidator : AbstractValidator<Voucher>
    {
        public static string AtivoErrorMsg = "O Voucher não é mais valido";
        
        public static string DataValidadeErrorMsg = "O Voucher está expirado";
        
        public static string UtilizadoErrorMsg = "O Voucher já foi utilizado";
        
        public static string DescricaoErrorMsg = "O Voucher com 'Descricao' invalido";
        
        public static string QuantidadeErrorMsg = "O Voucher não está mais disponivel";
        
        public static string ValorDescontoErrorMsg = "'ValorDesconto' está invalido";
        
        public static string PorcentualDescontoErrorMsg = "'PorcentualDesconto' está invalido";
        
        public VoucherValidator()
        {
            RuleFor(v => v.Descricao)
                .Must(descricao => !string.IsNullOrEmpty(descricao))
                .WithMessage(DescricaoErrorMsg);

            RuleFor(v => v.Ativo)
                .Equal(true)
                .WithMessage(AtivoErrorMsg);

            RuleFor(v => v.DataValidade)
                .NotEmpty()
                .NotNull()
                .Must(dataValidade => ValidarData(dataValidade))
                .Must(dataValidade => ValidarDataExpirada(dataValidade))
                .WithMessage(DataValidadeErrorMsg);

            RuleFor(v => v.Utilizado)
                .Equal(false)
                .WithMessage(UtilizadoErrorMsg);
            
            RuleFor(v => v.Quantidade)
                .GreaterThan(0)
                .WithMessage(QuantidadeErrorMsg);

            When(v => v.Tipo == TipoDescontoVoucher.Porcentual, () =>
            {
                RuleFor(v => v.PorcentualDesconto)
                    .NotNull()
                    .WithMessage(PorcentualDescontoErrorMsg)
                    .GreaterThan(0)
                    .WithMessage(PorcentualDescontoErrorMsg);
            });
            
            When(v => v.Tipo == TipoDescontoVoucher.Valor, () =>
            {
                RuleFor(v => v.ValorDesconto)
                    .NotNull()
                    .WithMessage(ValorDescontoErrorMsg)
                    .GreaterThan(0)
                    .WithMessage(ValorDescontoErrorMsg);
            });
        }

        public bool ValidarDataExpirada(DateTime data)
        {
            return  data > DateTime.Now;
        }
        
        public bool ValidarData(DateTime data)
        {
            return  data >= DateTime.Now.AddYears(-150);
        }
    }
}