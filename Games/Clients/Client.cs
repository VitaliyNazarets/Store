using Store.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Clients
{
	public class Client<IProduct> : IClient<IProduct>
	{
		public Task<IEnumerable<IProduct>> GetNewProductsAsync()
		{
			throw new System.NotImplementedException();
		}
	}
}
