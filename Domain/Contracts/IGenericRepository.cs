using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Domain.Contracts
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		T Find(object id);
		void Add(T entity);
		void Delete(T entity);
		void Edit(T entity);

		void AddRange(List<T> entities);
		void DeleteRange(List<T> entities);

		IEnumerable<T> GetAll();

		T FindFirstOrDefault(Expression<Func<T, bool>> predicate);

		IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

		IQueryable<T> FindBy(
			Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>,
			IOrderedQueryable<T>> orderBy = null,
			string includeProperties = ""
		 );

	}
}
