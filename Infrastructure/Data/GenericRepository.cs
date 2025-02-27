

using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T, TId>(StoreContext context) : IGenericRepository<T, TId> where T : BaseEntity<TId>
    {


        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public bool Exists(TId id)
        {
            return context.Set<T>().Any(x => x.Id.Equals(id));
        }

        public async Task<T?> GetByIdAsync(TId id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
