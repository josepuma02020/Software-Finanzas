using Domain.Base;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Base
{
    public class GenericRepository<T> : IGenericRepository<T>
           where T : BaseEntity
    {
        protected IDbContext Db;
        protected readonly DbSet<T> Dbset;


        public GenericRepository(IDbContext context)
        {
            Db = context;
            Dbset = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {

            return Dbset.AsEnumerable();
        }

        public virtual T Find(object id)
        {
            return Dbset.Find(id);
        }



        protected IQueryable<T> FindByAsQueryable(Expression<Func<T, bool>> predicate)
        {
            return Dbset.Where(predicate);
        }

        protected IQueryable<T> AsQueryable()
        {
            return Dbset.AsQueryable();
        }
        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = Dbset.Where(predicate).AsEnumerable();
            return query;
        }
        public virtual IQueryable<T> FindBy(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "")
        {
            IQueryable<T> query = Dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public T FindFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            T query = Dbset.FirstOrDefault(predicate);
            return query;
        }
        public virtual void Add(T entity)
        {
            Dbset.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Dbset.Remove(entity);
        }
        public virtual void Edit(T entity)
        {
            Db.SetModified(entity);
        }
        public virtual void DeleteRange(List<T> entities)
        {
            Dbset.RemoveRange(entities);
        }
        public virtual void AddRange(List<T> entities)
        {
            Dbset.AddRange(entities);
        }
    }
}
