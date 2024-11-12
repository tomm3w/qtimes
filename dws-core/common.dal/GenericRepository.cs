using System;
using System.Data.Entity;
using System.Linq;

namespace common.dal
{
	public abstract class GenericRepository<C, T> :
		IGenericRepository<C,T>
		where T : class
		where C : DbContext, new()
	{

		private C _entities = new C();
		public C Context
		{

			get { return _entities; }
			set { _entities = value; }
		}

		public virtual IQueryable<T> GetAll()
		{

			IQueryable<T> query = _entities.Set<T>();
			return query;
		}

		public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
		{

			IQueryable<T> query = _entities.Set<T>().Where(predicate);
			return query;
		}

		public virtual void Add(T entity)
		{
			_entities.Set<T>().Add(entity);
		}

        public T AddInstantly(T entity)
        {
            _entities.Set<T>().Add(entity);
            _entities.SaveChanges();
            return entity;
        }

		public virtual void Delete(T entity)
		{
			_entities.Set<T>().Remove(entity);
		}

		public virtual void Edit(T entity)
		{
			_entities.Entry(entity).State = EntityState.Modified;
		}

		public virtual void Save()
		{
			_entities.SaveChanges();
		}

		public bool Any(Func<T, bool> predicate)
		{
			var result = _entities.Set<T>().Any(predicate);
			return result;
		}
	}
}