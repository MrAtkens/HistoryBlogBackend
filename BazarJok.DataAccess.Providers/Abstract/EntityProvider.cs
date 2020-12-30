using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BazarJok.DataAccess.Providers.Abstract
{
    public abstract class EntityProvider<TContext, TEntity, TId>
        // Модель таблицы БД
        where TEntity : class

        // Контекст базы данных
        where TContext : DbContext

        // TId - тип идентификатора
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        protected EntityProvider(TContext context)
        {
            // Контекст базы данных
            this._context = context;

            // Достаём из контекста нужный нам DbSet<>
            this._dbSet = context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAll()
        {
            // Получаем все данные из таблицы, 
            // при этом не отслеживаем изменения этих моделей
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetById(TId id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<TEntity>> Get(Func<TEntity, bool> predicate)
        {
            // Получаем все модели таблицы и возвращаем только то, 
            // что подходит по условию, заданное в делегате
            return (await GetAll()).Where(predicate).ToList();
        }

        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task Add(TEntity added)
        {
            await _dbSet.AddAsync(added);
            await _context.SaveChangesAsync();
        }

        public async Task AddRange(IEnumerable<TEntity> added)
        {
            await _dbSet.AddRangeAsync(added);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(TEntity edited)
        {
            _context.Entry(edited).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Remove(TEntity removed)
        {
            _dbSet.Remove(removed);

            await _context.SaveChangesAsync();
        }
    }

}
