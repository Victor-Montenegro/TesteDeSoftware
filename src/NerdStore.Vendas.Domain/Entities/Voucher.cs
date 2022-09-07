using System;
using NerdStore.Vendas.Domain.Enums;
using NerdStore.Vendas.Domain.Validatories;

namespace NerdStore.Vendas.Domain.Entities
{
    public class Voucher : BaseEntity
    {
        public Guid Id { get; private set ; }
        public bool Ativo { get; private set ; }
        public bool Utilizado { get; private set ; }
        public int Quantidade { get; private set ; }
        public string Descricao { get; private set ; }
        public DateTime DataValidade { get; private set ; }
        public decimal? ValorDesconto { get; private set ; }
        public TipoDescontoVoucher Tipo { get; private set ; }
        public decimal? PorcentualDesconto { get; private set ; }

        public Voucher(Guid id, bool ativo, bool utilizado, string descricao, int quantidadeDiasDataValidade, TipoDescontoVoucher tipo, decimal? valorDesconto,  decimal? porcentualDesconto, int quantidade)
        {
            Id = id;
            Tipo = tipo;
            Ativo = ativo;
            Utilizado = utilizado;
            Descricao = descricao;
            Quantidade = quantidade;
            ValorDesconto = valorDesconto;
            PorcentualDesconto = porcentualDesconto;

            AdicionarDiasDataValidade(quantidadeDiasDataValidade);
        }

        private void AdicionarDiasDataValidade(int quantidadeDias)
        {
            DataValidade = DateTime.Now.AddDays(quantidadeDias);
        }

        public bool ValidarVoucher()
        {
            var validator = new VoucherValidator();
            ValidationResult = validator.Validate(this);

            return ValidationResult.IsValid;
        }

        public static class VoucherFactory
        {
            public static Voucher CriarVoucherValido()
                => new Voucher(Guid.NewGuid(),
                    true,
                    false,
                    "OFF15",
                    2,
                    TipoDescontoVoucher.Valor,
                    15,
                    null,
                    2);
        }
    }
}