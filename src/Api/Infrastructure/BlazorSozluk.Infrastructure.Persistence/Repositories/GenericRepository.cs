using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Api.Domain.Models;
using BlazorSozluk.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext dbContext;
        protected DbSet<TEntity> entity => dbContext.Set<TEntity>();

        public GenericRepository(DbContext context)
        {
            this.dbContext = context??throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.entity.SingleOrDefaultAsync(predicate);
        }

        public int Add(TEntity entity)
        {
           this.entity.Add(entity);
            return dbContext.SaveChanges();
        }

        public int Add(IEnumerable<TEntity> entities)
        {
            this.entity.AddRange(entities);
          return  dbContext.SaveChanges();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await this.entity.AddAsync(entity);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {

            await this.entity.AddRangeAsync(entities);
            return  dbContext.SaveChanges();
        }

        public int AddOrUpdate(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddOrUpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            throw new NotImplementedException();
        }

        public int Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual  Task<int> DeleteAsync(TEntity entity)
        {
            if (dbContext.Entry(entity).State==EntityState.Detached)
            {
                this.entity.Attach(entity);
            }
            this.entity.Remove(entity);
            return dbContext.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(Guid id)
        {
          var entity=this.entity.Find(id);
            return DeleteAsync(entity); 
        }

        public bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
           dbContext.RemoveRange(entity.Where(predicate));
            return dbContext.SaveChanges()>0;
        }
      
        public Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> GetAll(bool noTracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public virtual int Update(TEntity entity)
        {
           this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;   
            return dbContext.SaveChanges(); 
        }

        public async Task<int> UpdateEntity(TEntity entity)
        {
             this.entity.Attach(entity);
              dbContext.Entry(entity).State = EntityState.Modified;
            return dbContext.SaveChanges();
        }
    }
}
