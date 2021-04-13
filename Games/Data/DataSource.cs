using Store.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Data
{
	public class DataSource<T> : IDataSource<T>  where T : IProduct
	{
		private ConcurrentDictionary<string, T> _products = new ConcurrentDictionary<string, T>();
		public Task AddAsync(T value)
		{
			if (value.Name.Length < 2 || value.Price < 0 || value.Name.Length > 100)
				throw new ArgumentException();

			return Task.FromResult(_products.GetOrAdd(value.Name, value));
		}

		public Task<T> GetAsync(string name)
		{
			var t = _products.GetValueOrDefault(name);
			if (t is null)
				throw new ArgumentNullException();
			return Task.FromResult(t);
		}

		public Task<IEnumerable<T>> GetIenumerableAsync()
		{
			return Task.FromResult(_products.Select(f => f.Value));
		}

		public Task RemoveAsync(string name)
		{
			_products.TryRemove(name, out T t);
			if (t is null)
				throw new ArgumentNullException();
			return Task.FromResult(true);
		}

		public Task UpdateAsync(T value)
		{
			return Task.FromResult(_products.TryGetValue(value.Name, out T currentValue)
				? _products.TryUpdate(value.Name, value, currentValue) : throw new Exception($"Can't find IProduct: {value.Name}"));
		}
	}
}
