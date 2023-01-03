using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SqlServ4r.RepGenerationPatten
{
    public interface IGenericRepository<T,TKey> where T : class where TKey:struct
    {   
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> ToListAsync();
        List<T> ToList();
        Task<long> GetCountAsync();
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Add(T entity);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}