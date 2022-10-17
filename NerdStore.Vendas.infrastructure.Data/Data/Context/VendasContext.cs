using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NerdStore.Vendas.Core.Data;
using NerdStore.Vendas.infrastructure.Data.Extensions;

namespace NerdStore.Vendas.infrastructure.Data.Data.Context
{
    public class VendasContext : DbContext, IUnitOfWork
    {
        private IMediator _mediator;

        public VendasContext(DbContextOptions<VendasContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public async Task<bool> Commit()
        {
            var isSave = await SaveChangesAsync() > 0;

            if (isSave)
                _mediator.PublishEvent(this);

            return isSave;
        }

        public async Task Rollback()
        {
            await SaveChangesAsync();
        }
    }
}