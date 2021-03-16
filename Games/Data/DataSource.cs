using Store.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Data
{
	public class DataSource<IProduct> : IDataSource<IProduct>
	{
		public Task AddAsync(IProduct value)
		{
			throw new NotImplementedException();
		}

		public Task<IProduct> GetAsync(string name)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<IProduct>> GetListAsync()
		{
			throw new NotImplementedException();
		}

		public Task RemoveAsync(string name)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(IProduct value)
		{
			throw new NotImplementedException();
		}
	}
}
