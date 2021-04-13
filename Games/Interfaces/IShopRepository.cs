using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Interfaces
{
	public interface IShopRepository<T>
	{
		public Task<T> GetProductByNameAsync(string name);
		public Task UpdateNewProductsAsync();
		public decimal CalculateProductsPrice(IEnumerable<T> products);
		public Task<IEnumerable<T>> GetProductsAsync();
		public Task<bool> AddProductsAsync(IEnumerable<T> products);
		public Task UpdateProductAsync(T product);
	}
}
