using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repo.Interfaces
{
    public interface IGeneric<T> where T : class
    {
		T GetById(long id);

		IEnumerable<T> GetAll();

		IEnumerable<T> FindAll(Expression<Func<T, bool>> expression);

		T FindFirst(Expression<Func<T, bool>> expression);

		void Add(T entity);

		void AddRange(IEnumerable<T> entities);

		void Remove(T entity);

		void RemoveRange(IEnumerable<T> entities);

		void Update(T entity);
	}
}
