using Store.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Repositories
{
	public class ShopRepository<T> : IShopRepository<T> where T : IProduct
	{
		private readonly IClient<T> _client;
		private readonly IDataSource<T> _source;
		public ShopRepository(IClient<T> client, IDataSource<T> source)
		{
			_client = client;
			_source = source;
		}

		public async Task<IEnumerable<T>> GetProductsAsync() => await _source.GetListAsync().ConfigureAwait(false);

		public decimal CalculateProductsPrice(IEnumerable<T> products)
		{
			return products.Sum(f => f.Price);
		}

		public async Task<T> GetProductByNameAsync(string name)
		{
			return await _source.GetAsync(name).ConfigureAwait(false);
		}

		public async Task UpdateNewProductsAsync()
		{
			var products = await _client.GetNewProductsAsync().ConfigureAwait(false);
			foreach (var product in products)
			{
				await _source.AddAsync(product).ConfigureAwait(false);
			}
		}
	}
}
