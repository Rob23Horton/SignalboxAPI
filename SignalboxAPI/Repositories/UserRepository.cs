using MySqlConnector;
using Shared.Models;
using Signalbox.Shared.Models;
using SignalboxAPI.Interfaces;
using SignalboxAPI.Services;
using System.Linq;

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
			List<User> users = _databaseConnector.GetData<User>($"SELECT * FROM tblUser WHERE UserId = '{userId}';");

			if (users.Count() == 0)
			{
				throw new Exception();
			}

			return users[0];
		}


		public bool UserInGroup(int userId, int groupId)
		{
			List<UserGroup> usersGroups = _databaseConnector.GetData<UserGroup>($"SELECT * FROM tblUserGroup WHERE UserCode = '{userId}' AND GroupCode = '{groupId}';");

			if (usersGroups.Count() > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
