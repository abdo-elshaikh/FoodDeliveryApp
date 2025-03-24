using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FoodDeliveryApp.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        int SaveChanges();
    }
}