using Store.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Data
{
	public class UserSource : IUserSource
	{
		private readonly ConcurrentDictionary<int, IUser> _users = new ConcurrentDictionary<int, IUser>();

		public Task AddAsync(IUser user)
		{
			return Task.FromResult(_users.GetOrAdd(user.Id, user));
		}

		public Task<IUser> FindAsync(string email = "", string phoneNumber = "")
		{
			IUser user;
			if (string.IsNullOrEmpty(phoneNumber))
				user = _users.Where(f => f.Value.Email == email)?.FirstOrDefault().Value;
			else if (string.IsNullOrEmpty(email))
				user = _users.Where(f => f.Value.Phone == phoneNumber)?.FirstOrDefault().Value;
			else
				throw new Exception("Can't find customer without email and phone number");
			
			return Task.FromResult(user);
		}

		public Task<IUser> GetAsync(int id)
		{
			return Task.FromResult(_users.GetValueOrDefault(id));
		}

		public Task UpdateAsync(IUser user)
		{
			return Task.FromResult(_users.TryGetValue(user.Id, out IUser currentValue)
							? _users.TryUpdate(user.Id, user, currentValue) : 
							throw new Exception($"Can't find User by id: {user.Id}"));

		}
	}
}
