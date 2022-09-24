using FluentValidation.Results;
using MediatR;

namespace NerdStore.Vendas.Core.Messages
{
    public abstract class Request<T> : Message, IRequest<T> where T : Response
    {
        public ValidationResult ValidationResult { get; set; }

        public abstract bool EhValido();
    }
}