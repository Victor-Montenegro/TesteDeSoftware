using System.Threading.Tasks;

namespace NerdStore.Vendas.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool>  Commit();
        Task Rollback();
    }
}