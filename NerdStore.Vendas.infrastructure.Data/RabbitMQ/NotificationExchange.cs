using System.Threading.Tasks;
using MediatR;
using NerdStore.Vendas.Core.Interfaces;

namespace NerdStore.Vendas.infrastructure.Data.RabbitMQ
{
    public class NotificationExchange : IExchangeNotification
    {
        public async Task Publish(INotification notification)
        {
        }
    }
}