using System.Threading.Tasks;

namespace NerdStore.Vendas.Core.Data
{
    public interface IUnitOfWork
    {
        Task Commit();
        Task Rollback();
    }
}