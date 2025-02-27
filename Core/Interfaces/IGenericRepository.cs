

using Core.Entities;

namespace Core.Interfaces
{
    public interface IGenericRepository<T,TId> where T : BaseEntity<TId>
    {
        Task<T?> GetByIdAsync(TId id);
        Task<IReadOnlyList<T>> ListAllAsync();
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<bool> SaveAllAsync();
        bool Exists(TId id);
    }
}
