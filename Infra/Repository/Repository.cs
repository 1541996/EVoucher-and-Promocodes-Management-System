using Core.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //protected DbSet<T> dBset;
        private readonly DbContext _dbContex;

        public Repository(DbContext dbContex)
        {
            this._dbContex = dbContex;

        }

        public IQueryable<T> GetAll()
        {
            return _dbContex.Set<T>().AsQueryable();

        }
        public IQueryable<T> Query(Expression<Func<T, bool>> filter, bool showDeleted = false)
        {
            return _dbContex.Set<T>().Where(filter).AsQueryable();
        }
        public int Insert(T entity)
        {
            _dbContex.Set<T>().Add(entity);
            return _dbContex.SaveChanges();
        }

        public T InsertReturn(T entity)
        {
            T newEntity = _dbContex.Set<T>().Add(entity).Entity;
            var result = _dbContex.SaveChanges();
            if (result > 0)
            {
                return newEntity;
            }
            else
            {
                return null;
            }
        }

        public async Task<T> InsertReturnAsync(T entity)
        {
            T newEntity = _dbContex.Set<T>().Add(entity).Entity;
            var result = await _dbContex.SaveChangesAsync();
            if (result > 0)
            {
                return newEntity;
            }
            else
            {
                return null;
            }
        }

        public int Delete(T entity)
        {
            _dbContex.Set<T>().Remove(entity);
            return _dbContex.SaveChanges();
        }

        public int Update(T entity)
        {
            
               // _dbContex.Entry(entity).State = EntityState.Modified;
                return _dbContex.SaveChanges();

           
        }



        public T UpdatePartial(T OldEntity, T NewEntity, params Expression<Func<T, object>>[] propertiesToUpdate)
        {
            _dbContex.Entry(OldEntity).State = EntityState.Detached;
            _dbContex.Set<T>().Attach(NewEntity);

            foreach (var p in propertiesToUpdate)
            {
                _dbContex.Entry(NewEntity).Property(p).IsModified = true;
            }

            var result = _dbContex.SaveChanges();
            if (result > 0)
            {
                return NewEntity;
            }
            else
            {
                return null;
            }
        }



        public T UpdateComplete(T OldEntity, T NewEntity)
        {
            try
            {
                _dbContex.Entry(OldEntity).State = EntityState.Detached;
                _dbContex.Set<T>().Attach(NewEntity);
                _dbContex.Entry(NewEntity).State = EntityState.Modified;

                var result = _dbContex.SaveChanges();
                if (result > 0)
                {
                    return NewEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            return null;

        }

        public async Task<T> UpdateCompleteAsync(T OldEntity, T NewEntity)
        {
            _dbContex.Entry(OldEntity).State = EntityState.Detached;
            _dbContex.Set<T>().Attach(NewEntity);
            _dbContex.Entry(NewEntity).State = EntityState.Modified;

            var result = await _dbContex.SaveChangesAsync();
            if (result > 0)
            {
                return NewEntity;
            }
            else
            {
                return null;
            }
        }

        public T GetById(int id)
        {
            return _dbContex.Set<T>().Find(id);
        }

        public T GetByCompositeKey(int id, string key)
        {

            return _dbContex.Set<T>().Find(id, key);

        }
        public int MaxNumber(Expression<Func<T, int>> expression)
        {
            return this._dbContex.Set<T>().Select(expression).Max();
        }


        public IQueryable<T> Take(int count)
        {
            return this._dbContex.Set<T>().Take(count).AsQueryable();
        }
        public IQueryable<T> Skip(int count)
        {
            return this._dbContex.Set<T>().Skip(count).AsQueryable();
        }
        public int Remove(T entity)
        {

            return this._dbContex.SaveChanges();
        }


        public int Count(Expression<Func<T, bool>> filter, Expression<Func<T, int>> field)
        {

            return _dbContex.Set<T>().Where(filter).Select(field).Count();
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContex.Dispose();
            }
            this.Dispose(disposing);
        }

        public IQueryable<T> OrderBy(Expression<Func<T, string>> filter)
        {
            return _dbContex.Set<T>().OrderBy(filter).AsQueryable();
        }
        public T UpdateWithObj(T entity)
        {

            try
            {
             //   _dbContex.Entry<T>(entity).State = EntityState.Detached;
             //   _dbContex.Set<T>().Attach(entity);
                _dbContex.Entry<T>(entity).State = EntityState.Modified;
                if (_dbContex.SaveChanges() > 0)
                {
                    return entity;
                }


            }
            catch (Exception e)
            {

            }
            return null;


        }




    }
}
