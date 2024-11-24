using Shared.Models;
using Signalbox.Shared.Models;
using SignalboxAPI.DatabaseModels;
using SignalboxAPI.Interfaces;
using SignalboxAPI.Services;

namespace SignalboxAPI.Repositories
{
	public class GroupRepository : IGroupRepository
	{
		private readonly IDatabaseConnector _databaseConnector;

		public GroupRepository(IDatabaseConnector databaseConnector)
		{
			this._databaseConnector = databaseConnector;
		}

		public Group CreateGroup(string groupName)
		{
			SqlRequest groupRequest = new SqlRequest("tblGroup");

			SqlData groupData = new SqlData("tblGroup", "Name", groupName);
			groupRequest.RequestValues.Add(groupData);

			//Checks if group name already exists
			if (_databaseConnector.SelectData<Group>(groupRequest).Count() > 0)
			{
				throw new Exception();
			}

			_databaseConnector.InsertData(groupRequest);

			List<Group> group = _databaseConnector.SelectData<Group>(groupRequest);

			if (group.Count() == 0)
			{
				throw new Exception();
			}

			return group[0];
		}

		public Group CreateGroupFromUsers(string groupName, List<int> userIds)
		{
			Group group = this.CreateGroup(groupName);

			SqlData groupData = new SqlData("tblUserGroup", "GroupCode", group.GroupId);

			foreach (int userId in userIds)
			{

				SqlRequest userRequest = new SqlRequest("tblUserGroup");
				userRequest.RequestValues.Add(groupData);

				SqlData userData = new SqlData("tblUserGroup", "UserCode", userId);
				userRequest.RequestValues.Add(userData);

				_databaseConnector.InsertData(userRequest);
			}

			return group;
		}

		public UserAndGroups GetAllGroupsForUser(int userId)
		{
			UserAndGroups user = new UserAndGroups();

			SqlRequest userRequest = new SqlRequest("tblUser");

			SqlData userData = new SqlData("tblUser", "UserId", userId);
			userRequest.RequestValues.Add(userData);

			List<User> users = _databaseConnector.SelectData<User>(userRequest);

			if (users.Count() == 0)
			{
				throw new Exception();
			}

			user.UserId = userId;
			user.Name = users[0].Name;
			user.UserName = users[0].UserName;

			SqlRequest groupsRequest = new SqlRequest("tblGroup");

			SqlJoin groupJoin = new SqlJoin("tblGroup", "GroupId", "tblUserGroup", "GroupCode");
			groupsRequest.Joins.Add(groupJoin);

			userData = new SqlData("tblUserGroup", "UserCode", userId);
			groupsRequest.RequestValues.Add(userData);

			List<Group> groups = _databaseConnector.SelectData<Group>(groupsRequest);

			user.Groups = groups;

			return user;
		}

		public List<User> GetAllUsersInGroup(int groupId)
		{
			SqlRequest usersRequest = new SqlRequest("tblUser");

			SqlJoin groupJoin = new SqlJoin("tblUser", "UserId", "tblUserGroup", "UserCode");
			usersRequest.Joins.Add(groupJoin);

			SqlData groupData = new SqlData("tblUserGroup", "GroupCode", groupId);
			usersRequest.RequestValues.Add(groupData);

			List<User> users = _databaseConnector.SelectData<User>(usersRequest);

			if (users.Count() == 0)
			{
				throw new Exception();
			}

			return users;
		}

		public Group GetGroupDetailsFromId(int groupId)
		{
			SqlRequest groupRequest = new SqlRequest("tblGroup");

			SqlData groupData = new SqlData("tblGroup", "GroupId", groupId);
			groupRequest.RequestValues.Add(groupData);

			List<Group> group = _databaseConnector.SelectData<Group>(groupRequest);

			if (group.Count() == 0)
			{
				throw new Exception();
			}

			return group[0];
		}
	}
}
