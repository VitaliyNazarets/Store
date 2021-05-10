using Store.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Interfaces
{

	public interface IUserAdapter
	{
		public Task RegisterAsync(IUser user);

		public Task UpgradeAsync(IUser user, UserState state);

		public Task CloseByEmailAsync(string email);
		public Task CloseByPhoneAsync(string phone);
		public Task UpdateEmail(IUser user, string email);

	}
}
