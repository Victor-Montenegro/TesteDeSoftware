using FluentValidation;
using NerdStore.Vendas.Domain.Entities;

namespace NerdStore.Vendas.Domain.Validatories
{
    public class PedidoItemValidator : AbstractValidator<PedidoItem>
    {
        public PedidoItemValidator()
        {
            RuleFor(p => p.Quantidade)
                .GreaterThan(1)
                .LessThanOrEqualTo(15)
                .WithMessage("O item de pedido deve ter a unidade de 1 a 15 unidades");
        }
    }
}