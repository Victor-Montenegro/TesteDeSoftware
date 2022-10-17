using System.Threading.Tasks;
using MediatR;

namespace NerdStore.Vendas.Core.Interfaces
{
    public interface IExchangeNotification
    {
        Task Publish(INotification notification);
    }
}