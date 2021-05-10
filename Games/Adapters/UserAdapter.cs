using Store.Enums;
using Store.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Adapters
{
	public class UserAdapter : IUserAdapter
	{
		private readonly IUserSource _userSource;
		public UserAdapter(IUserSource userSource)
		{
			_userSource = userSource;
		}

		public async Task CloseByEmailAsync(string email)
		{
			var userToClose = await _userSource.FindAsync(email: email);
			if (userToClose is null)
				throw new Exception("Can't find profile");

			userToClose.UserState = UserState.Closed;
			await _userSource.UpdateAsync(userToClose);
		}

		public async Task CloseByPhoneAsync(string phone)
		{
			var userToClose = await _userSource.FindAsync(phoneNumber: phone);
			if (userToClose is null)
				throw new Exception("Can't find profile");

			userToClose.UserState = UserState.Closed;
			await _userSource.UpdateAsync(userToClose);
		}

		public async Task RegisterAsync(IUser user)
		{
			if (user.Phone is null || user.Email is null)
				throw new Exception("Invalid parameters");
			user.UserState = UserState.NotVerified;
			await _userSource.AddAsync(user);
		}

		public async Task UpgradeAsync(IUser user, UserState state)
		{
			var userToUpdate = await _userSource.FindAsync(phoneNumber: user.Phone);
			if (userToUpdate is null)
				userToUpdate = await _userSource.FindAsync(email: user.Email);
			if (userToUpdate is null)
				throw new Exception("User not found");
			if (userToUpdate.UserState == UserState.Closed)
				throw new Exception("Profile is closed.");

			userToUpdate.UserState = state;
			await _userSource.UpdateAsync(userToUpdate);
		}

		public async Task UpdateEmail(IUser user, string email)
		{
			if (user.UserState == UserState.Closed)
				throw new Exception("User profile is closed.");
			user.Email = email;
			await _userSource.UpdateAsync(user);
		}
	}
}
