using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Interfaces
{
	public interface IDataSource<T>
	{
		public Task<T> GetAsync(string name);
		public Task AddAsync(T value);
		public Task RemoveAsync(string name);
		public Task UpdateAsync(T value);
		public Task<IEnumerable<T>> GetIenumerableAsync();
	}
}
