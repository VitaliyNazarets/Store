using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Interfaces
{
	public interface IClient<T>
	{
		public Task<IEnumerable<T>> GetNewProductsAsync();
	}
}
