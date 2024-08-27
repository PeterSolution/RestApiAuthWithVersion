using CoreApiInNet.Data;
using CoreApiInNet.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoreApiInNet.Contracts
{
    public interface InterfaceGenericReposotory<T> where T: class
    {
        Task<T> GetAsync(int? id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
        Task<bool> Exist(int id);
        Task<IDbContextTransaction> StartTransaction();
    }
    
}
