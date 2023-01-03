using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SqlServ4r.EntityFramework;
using Volo.Abp.DependencyInjection;

namespace SqlServ4r.RepGenerationPatten
{
    public class GenericRepository<T, Key> : IGenericRepository<T, Key> where T : class where Key : struct
    {
        public readonly DreamContext _context; 

        public GenericRepository(DreamContext context)
        {
            _context = context;
        }

        public DbSet<T> GetQueryable()
        {
            return _context.Set<T>();
        }


        public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(expression);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        
        public async Task<List<T>> ToListAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public List<T> ToList()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<long> GetCountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            _context.SaveChanges();
        }

        public void Add(T entity)
        {   
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync(); 
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChanges();
        }
    }
}