using System;
using NerdStore.Vendas.Core.Interfaces;

namespace NerdStore.Vendas.Core.Data
{
    public interface IRepository<TEntity> : IDisposable where TEntity : IAggregateRoot
    {
        public IUnitOfWork UnitOfWork { get; }
    }
}