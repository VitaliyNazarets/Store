using Store.Enums;

namespace Store.Interfaces
{
	public interface IUser
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public UserState UserState { get; set; }

	}
}
