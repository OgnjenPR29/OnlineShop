using DataLayer.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repo
{
    public class Generic<T> : IGeneric<T> where T : class
    {
		protected readonly OnlineShopContext _context;

		private readonly object balanceLock = new object();

		public Generic(OnlineShopContext context)
		{
			_context = context;
		}

		public void Add(T entity)
		{
			lock (balanceLock)
			{
				_context.Set<T>().Add(entity);
			}
		}

		public void AddRange(IEnumerable<T> entities)
		{
			lock (balanceLock)
			{
				_context.Set<T>().AddRange(entities);
			}
		}

		public IEnumerable<T> FindAll(Expression<Func<T, bool>> expression)
		{
			lock (balanceLock)
			{
				var result = _context.Set<T>().Where(expression).ToList();

				return result;
			}
		}

		public T FindFirst(Expression<Func<T, bool>> expression)
		{
			lock (balanceLock)
			{
				var result = _context.Set<T>().Where(expression).FirstOrDefault();

				return result;
			}
		}

		public IEnumerable<T> GetAll()
		{
			lock (balanceLock)
			{
				var result = _context.Set<T>().ToList();

				return result;
			}
		}

		public T GetById(long id)
		{
			lock (balanceLock)
			{
				var result = _context.Set<T>().Find(id);

				return result;
			}
		}

		public void Remove(T entity)
		{
			lock (balanceLock)
			{
				_context.Set<T>().Remove(entity);
			}
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			lock (balanceLock)
			{
				_context.Set<T>().RemoveRange(entities);
			}
		}

		public void Update(T entity)
		{
			lock (balanceLock)
			{
				_context.Set<T>().Update(entity);
			}
		}
	}
}
