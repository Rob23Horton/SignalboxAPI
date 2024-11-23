using Signalbox.Shared.Models;
using SignalboxAPI.Interfaces;
using SignalboxAPI.Services;

namespace SignalboxAPI.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly IDatabaseConnector _databaseConnector;

		public UserRepository(IDatabaseConnector databaseConnector)
		{
			this._databaseConnector = databaseConnector;
		}

		public User GetUserDetailsFromId(int userId)
		{
			throw new NotImplementedException();
			return new User();
		}

		public bool UserInGroup(int userId, int groupId)
		{
			throw new NotImplementedException();
			return false;
		}
	}
}
