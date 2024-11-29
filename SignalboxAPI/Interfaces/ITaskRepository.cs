using Signalbox.Shared.Models;

namespace SignalboxAPI.Interfaces
{
	public interface ITaskRepository
	{
		public UserTask CreateTaskFromGroupId(int groupId, string taskName, int taskTypeCode);
		public UserTask GetTaskDetailsFromId(int taskId);
		public GroupTasks GetTasksFromGroupId(int groupId);
		public List<User> GetTaskOwnerFromId(int taskId);
		public void SetTaskOwnerFromUserId(int taskId, int userId = 0);
		public void SetTaskOwnerFromGroupId(int taskId, int groupId);
	}
}
