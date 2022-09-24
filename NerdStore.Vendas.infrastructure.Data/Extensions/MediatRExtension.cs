using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NerdStore.Vendas.Core.Data;
using NerdStore.Vendas.infrastructure.Data.Data.Context;

namespace NerdStore.Vendas.infrastructure.Data.Extensions
{
    public static class MediatRExtension
    {
        public async static void PublishEvent(this IMediator mediator, VendasContext context)
        {
            var entities = context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(w => w.Entity.Notification != null && w.Entity.Notification.Any());

            var events = entities
                .SelectMany(s => s.Entity.Notification)
                .ToList();
            
            entities.ToList()
                .ForEach(e => e.Entity.LimparEventos());
            
            var tasks = events.Select(async (s) => { await mediator.Publish(s); });

            await Task.WhenAll(tasks);
        }
    }
}