using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NerdStore.Vendas.Core.Data;
using NerdStore.Vendas.Core.Interfaces;

namespace NerdStore.Vendas.Core.DomainObject
{
    public abstract class RequestHandler<TCommand, TCommandResult> : IRequestHandler<TCommand, TCommandResult>
        where TCommand : Command<TCommandResult>
    {
        private IExchangeNotification _exchangeNotification;

        public RequestHandler(IExchangeNotification exchangeNotification)
        {
            _exchangeNotification = exchangeNotification;
        }

        public abstract Task<TCommandResult> Handle(TCommand request, CancellationToken cancellationToken);

        protected async Task<bool> ValidarCommand(TCommand command)
        {
            var result = command.EhValido();

            if (!result)
            {
               var tasks =  command.ValidationResult.Errors.Select(async f =>
                    await PublishNotification(f.ErrorCode, f.ErrorMessage));

               Task.WaitAll(tasks.ToArray());
            }
            
            return result;
        }

        private async Task PublishNotification(string key, string value)
        {
            var notification = new DomainNotification(DateTime.Now, Guid.NewGuid(), key, value);

            await _exchangeNotification.Publish(notification);
        }
    }
}