using Signalbox.Shared.Models;

namespace SignalboxAPI.Interfaces
{
	public interface IUserRepository
	{
		public User GetUserDetailsFromId(int userId);
		public bool UserInGroup(int userId, int groupId);
	}
}
