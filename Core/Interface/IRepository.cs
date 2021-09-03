using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IRepository<T>
    {
        int MaxNumber(Expression<Func<T, int>> expression);
        int Count(Expression<Func<T, bool>> filter, Expression<Func<T, int>> field);
        IQueryable<T> GetAll();
        int Insert(T entity);
        T InsertReturn(T entity);
        Task<T> InsertReturnAsync(T entity);
        int Delete(T entity);
        int Update(T entity);

        //T NewUpdate(string method, T OldEntity, T NewEntity, params Expression<Func<T, object>>[] propertiesToUpdate);
        T UpdatePartial(T OldEntity, T NewEntity, params Expression<Func<T, object>>[] propertiesToUpdate);
        T UpdateComplete(T OldEntity, T NewEntity);
        Task<T> UpdateCompleteAsync(T OldEntity, T NewEntity);
        int Remove(T entity);
        T GetById(int id);
        T GetByCompositeKey(int id, string key);
        IQueryable<T> Query(Expression<Func<T, bool>> filter, bool showDeleted = false);
        IQueryable<T> Take(int count);
        IQueryable<T> Skip(int count);
        IQueryable<T> OrderBy(Expression<Func<T, string>> filter);
        void Dispose(bool disposing);

        T UpdateWithObj(T entity);
    }
}
