using MySqlConnector;
using Shared.Models;
using Signalbox.Shared.Models;
using SignalboxAPI.DatabaseModels;
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
			SqlRequest userRequest = new SqlRequest("tblUser");
			SqlData userIdData = new SqlData("tblUser", "UserId", userId);
			userRequest.RequestValues.Add(userIdData);

			List<User> users = _databaseConnector.SelectData<User>(userRequest);

			if (users.Count() == 0)
			{
				throw new Exception();
			}

			return users[0];
		}


		public bool UserInGroup(int userId, int groupId)
		{

			SqlRequest request = new SqlRequest("tblUserGroup");

			SqlData userCode = new SqlData("tblUserGroup", "UserCode", userId);
			request.RequestValues.Add(userCode);

			SqlData groupCode = new SqlData("tblUserGroup", "GroupCode", groupId);
			request.RequestValues.Add(groupCode);

			List<UserGroup> usersGroups = _databaseConnector.SelectData<UserGroup>(request);

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
