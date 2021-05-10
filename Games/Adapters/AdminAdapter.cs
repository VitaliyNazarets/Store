using Store.Enums;
using Store.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Adapters
{
	public class AdminAdapter : UserAdapter, IAdminAdapter
	{
		private readonly IUserSource _userSource;
		private readonly IDataSource<IProduct> _productSource;

		public AdminAdapter(IUserSource userSource, IDataSource<IProduct> productSource) : base(userSource)
		{
			_productSource = productSource;
			_userSource = userSource;
		}

		public async Task UpdateProductsAsync(IUser user, IEnumerable<IProduct> products)
		{
			var userProfile = await _userSource.FindAsync(phoneNumber: user.Phone);
			if (userProfile is null)
				userProfile = await _userSource.FindAsync(email: user.Email);
			if (userProfile is null)
				throw new Exception("User not found");
			if (userProfile.UserState != UserState.Admin)
				throw new Exception("Forbidden");

			List<Task> tasks = new List<Task>();
			foreach (var product in products)
				tasks.Add(_productSource.AddAsync(product));

			await Task.WhenAll(tasks);
		}

	}
}
