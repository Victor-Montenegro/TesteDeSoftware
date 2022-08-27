using FluentValidation;
using NerdStore.Vendas.Domain.Entities;

namespace NerdStore.Vendas.Domain.Validatories
{
    public class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator()
        {
            // RuleFor(p => p.)
        }
    }
}