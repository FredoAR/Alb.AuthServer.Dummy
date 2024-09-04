using Alb.AuthServer.Infrastructure.EntityFrameworkCore.Context;
using Alb.Identity.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Alb.AuthServer.Infrastructure.EntityFrameworkCore.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AuthServerDbContext _authServerDbContext;
        private DbSet<T> _dbSetEntities;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authServerDbContext">Contexto definido para autenticación y autorización de usuarios</param>
        public Repository(AuthServerDbContext authServerDbContext)
        {
            _authServerDbContext = authServerDbContext;
            _dbSetEntities = _authServerDbContext.Set<T>();
        }


        public async Task AddAsync(T entity)
        {
            await _dbSetEntities.AddAsync(entity);
            await _authServerDbContext.SaveChangesAsync();
        }


        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSetEntities.AddRangeAsync(entities);
            await _authServerDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSetEntities.FindAsync(id);
            if (entity != null)
            {
                _dbSetEntities.Remove(entity);
                await _authServerDbContext.SaveChangesAsync();
            }
        }

        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSetEntities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSetEntities.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSetEntities.Update(entity);
            await _authServerDbContext.SaveChangesAsync();
        }
    }
}
