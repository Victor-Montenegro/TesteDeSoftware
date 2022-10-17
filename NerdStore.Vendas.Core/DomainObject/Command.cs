using FluentValidation.Results;
using MediatR;

namespace NerdStore.Vendas.Core.Data
{
    public abstract class Command<T> : IRequest<T>
    {
        public ValidationResult ValidationResult { get; set; }

        public abstract bool EhValido();
    }
}