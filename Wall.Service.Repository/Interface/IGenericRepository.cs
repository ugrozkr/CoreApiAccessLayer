using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Wall.Service.Repository
{
    public interface IGenericRepository
    {
        IEnumerable<T> GetAll<T>() where T : class;
        IQueryable GetAll<T>(Expression<Func<T, bool>> predicate) where T : class; 
        T GetById<T>(Guid id) where T : class;
        T Get<T>(Expression<Func<T, bool>> predicate) where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Save();
    }
}
