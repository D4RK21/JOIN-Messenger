using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core;
using DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class GenericRepositoryDb<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppContext _appContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepositoryDb(AppContext appContext)
        {
            this._appContext = appContext;
            this._dbSet = appContext.Set<TEntity>();
        }

        // public async Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression)
        // {
        //     IQueryable<TEntity> query = _dbSet;
        //
        //     if (expression != null)
        //     {
        //         query = query.Where(expression);
        //     }
        //
        //     IEnumerable<TEntity> result = query;
        //
        //     return result;
        // }

        public IList<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TEntity>> selector = null)
        {
            IQueryable<TEntity> query = _dbSet;

            // var type = typeof(TEntity);
            // var props = type.GetProperties();

            List<TEntity> result;

            if (selector != null)
            {
                result = query.Where(predicate)
                    .Select(selector)
                    .ToList();
            }
            else
            {
                result = query.Where(predicate).ToList();
            }

            return result;
        }


        public async Task<TEntity> GetEntityById(int id)
        {
            var queryResult = await _dbSet.FindAsync(id);
            return queryResult;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async void Update(TEntity entityToUpdate)
        {
            _dbSet.Update(entityToUpdate);
        }

        // public void Update(TEntity entityToUpdate)
        // {
        //     _dbSet.Attach(entityToUpdate);
        //     _appContext.Entry(entityToUpdate).State = EntityState.Modified;
        // }

        public async void Delete(TEntity entityToDelete)
        {
            _dbSet.Remove(entityToDelete);
        }

        // public async Task Delete(TEntity entityToDelete)
        // {
        //     if (_appContext.Entry(entityToDelete).State == EntityState.Detached)
        //     {
        //         _dbSet.Attach(entityToDelete);
        //     }
        //     _dbSet.Remove(entityToDelete);
        // }
    }
}
