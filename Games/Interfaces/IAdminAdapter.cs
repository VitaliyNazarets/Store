using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Interfaces
{
	public interface IAdminAdapter
	{
		public Task UpdateProductsAsync(IUser user, IEnumerable<IProduct> products);
	}
}
