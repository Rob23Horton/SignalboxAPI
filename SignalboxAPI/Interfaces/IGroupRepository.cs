using Shared.Models;
using Signalbox.Shared.Models;

namespace SignalboxAPI.Interfaces
{
	public interface IGroupRepository
	{
		public Group GetGroupDetailsFromId(int groupId);
		public List<User> GetAllUsersInGroup(int groupId);
		public UserAndGroups GetAllGroupsForUser(int userId);
		public Group CreateGroup(string groupName);
		public Group CreateGroupFromUsers(string groupName, List<int> userIds);
	}
}
