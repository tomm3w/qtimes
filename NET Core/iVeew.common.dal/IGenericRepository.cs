using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace IVeew.common.dal
{
    public interface IGenericRepository<C, T>
		where T : class
		where C : DbContext, new()
	{

		IQueryable<T> GetAll();
		
		IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
		
		void Add(T entity);
        T AddInstantly(T entity);
       
		void Delete(T entity);
		
		void Edit(T entity);
		
		void Save();
		bool Any(Func<T, bool> predicate);
		C Context { get; set; }
	}
}