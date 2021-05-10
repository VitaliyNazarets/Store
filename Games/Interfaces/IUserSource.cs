using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Interfaces
{
	public interface IUserSource
	{
		public Task<IUser> GetAsync(int id);
		public Task<IUser> FindAsync(string email = "", string phoneNumber = "");
		public Task UpdateAsync(IUser user);
		public Task AddAsync(IUser user);
	}
}
