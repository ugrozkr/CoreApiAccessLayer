using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Wall.Service.DataAccessLayer.DbContexts;

namespace Wall.Service.Repository
{
    public class EFRepository : IGenericRepository
    {
        IServiceScopeFactory _serviceScopeFactory;
        public EFRepository(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                return dbContext.Set<T>();
            }          
        }

        public IQueryable GetAll<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                return dbContext.Set<T>().Where(predicate);
            }
        }

        public T GetById<T>(Guid id) where T : class
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                return dbContext.Set<T>().Find(id);
            }
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                return dbContext.Set<T>().Where(predicate).SingleOrDefault();
            }
        }

        public void Add<T>(T entity) where T : class
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                dbContext.Set<T>().Add(entity);
            }
        }

        public void Update<T>(T entity) where T : class
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                dbContext.Set<T>().Attach(entity);
                dbContext.Entry(entity).State = EntityState.Modified;
            }        
        }

        public void Delete<T>(T entity) where T : class
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                EntityEntry dbEntityEntry = dbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    dbContext.Set<T>().Attach(entity);
                    dbContext.Set<T>().Remove(entity);
                }
            }       
        }

        public void Save()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                dbContext.SaveChanges();
            }
        }
    }
}
